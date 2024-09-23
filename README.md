# Arstive
音乐游戏**Arstive**原型机，非游戏引擎开发，
运行于Windows平台，依赖.Net8.0框架，以
Windows Presentation Foundation为技术栈
进行开发。

## 简介
Arstive是以几何为特色的创新性下落式音游，判定依靠异形判定线“判定角”来进行，包含三种下落式音符Tap、Hold与Drag，以及一种自由音符（目前）Move，该音符给出两个按键，玩家应该在限定时间内由头部按键以随意路径滑动至尾部按键。<br/>
开发基于传统的三层模型，数据使用Json格式存储，但是方便起见直接将读取方法写在Chart加载类里导致了DAL和BLL产生了部分合并，UI层使用MVC模型，主窗口分数等信息直接以数据绑定（单向）的方式绑定在相应静态类上。<br/>
动画使用WPF动画框架，Note仅有线性下落动画，判定角支持移动，旋转，透明度动画，判定角这些动画均支持缓动函数，由Easing结构定义。这些缓动来自于WPF动画系统，包括BackEase、BounceEase、CircleEase、CubicEase、ElasticEase、ExponentialEase、PowerEase、QuadraticEase、QuinticEase、SineEase等。<br/>
判定角每个角Angle均绑定一个键盘按键Key，满足唯一映射关系f:Angle→Key，也就是说，每个角只能绑定一个键，同一个键可以绑定到多个角上。当Tap音符接近指定角时，需要打击角绑定的按键，当Drag音符接近相应角时，只需在接触时按住需要打击角绑定的按键即可，Hold音符最为复杂，需要在其存在时打击一直按住按键直至结束。

## 运行原理
本音游原型基于WPF开发，其底层运用DirectX引擎进行相应渲染。
<br/>
主要运作流程使用多线程技术进行管理，其中n个线程管理其中的每个判定角进行下落式音、符的判定，1个线程进行计时器的维护，1个线程用于Move音符的判定，一个现场进行音频的播放，以上线程在运行之前均使用一个Barrier阻塞，当所有线程均加载完毕后，Barrier被释放，游戏开始。<br/>
也就是说，每个判定角线程以10毫秒为周期震荡，并维护如下过程：
- 读取Note，计算该Note的下落时长并通过创建**ThicknessAnimation**进行Note下落动画的展示。
- 计算当前判定列表中最近的（三）个Note的判定时间与当前时间的差值，并根据按键状态进行对应的判定（非游戏引擎下的高效触控系统的打造详见后文），并对分数进行操作。
- 查找最近的几个判定角动画，并创建**ThicknessAnimation**或**DoubleAnimation**进行判定线动画的播放。

重复以上过程直到动画列表、Note列表均为空，此时线程积极触发Barrier，线程堵塞和挂起，直到所有线程均挂起，游戏结束。

## 相应实现
- **时钟系统**<br/>
  时钟系统由单独的线程进行维护，该系统以异步方式运行，每隔10毫秒计算开始时间与当前时间的差（为什么不直接对Tick进行+=10操作呢，因为这个10毫秒实质上没那么稳定，可能会有一点误差，对于音游而言，这些误差是致命的，所以采用稳定的系统时钟保证运行的稳定）并将其赋值给Tick。
  ```
  static async void CountTime()
  {
      // Ensure that the timing and judgment threads run simultaneously
      _barrier!.SignalAndWait();
  
      // Record start time
      var startTime = DateTime.Now;

      // Counting time elapsed asynchronous
      var periodicTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(10));
      while (await periodicTimer.WaitForNextTickAsync())
      {
          Tick = (int)(DateTime.Now - startTime).TotalMilliseconds;
      }
  }
  ```
- **缓冲池**<br/>
  有时候，第一个Note从创建到判定的时间差小于音乐的总播放时间（i.e. 某Tap被设置为在500ms时判定，而下落时间为2000ms），这时就会导致严重的偏差，这时应该使计数时间从-2500开始，-2500到0ms这段时间称为缓冲时间，这段时间内的线程上下文称之为缓冲池。一般地，应计算最初几个Note的差并给予Tick初始负值。
    ```
    var flootTime = 1410 / (double)(200 * (judgmentAngle.Speed)) * 1000;
    if (earliestTime >= judgmentAngle.NoteLists![0].HitTime - (int)(flootTime))
    {
        // Negative
        earliestTime = judgmentAngle.NoteLists[0].HitTime - (int)(flootTime);
    }
    ```
  另一方面，由于计数线程计算的Tick是开始时间与系统的直接差，所以应保存这个负值时间，并应用在Tick上。
  ```
  var interruptChange = 0;
  if (Tick < 0)
  {
      interruptChange = Tick;
  }
  Tick = (int)(DateTime.Now - startTime).TotalMilliseconds + interruptChange;
  ```
- **全局坐标与判定角坐标**<br/>
  我们知道，下落式音符是作为判定角的子级创建的，定位使用判定角坐标（JP），而判定角和自由音符是直接根据游戏窗体创建的，定位使用全局坐标（GP）。GP系统以左上角为原点(0,0)，右下角为最大坐标(w,h)，其中w和h代表窗口的长度的宽度。下落式音游通常拥有轨道的概念，事实上，判定角也是指定长度的不可见轨道，其长度为以w和h为直角边三角形的斜边长与Note的高度之和d。这样可以避免Note以可视化的形式创建造成灾难性的观感。则易知Note的初始位置应该为JP系统下的(0,-d,0,0)的外边距坐标，同时，该方案可以判定角移动时，确保Note下落的稳定，隔离了GP和JP系统，保证读谱的舒适度 
  ```
    // Need create
    if (deltaAnimation <= actualFlootTime * 1000)
    {
        // Create note instance
        UserControl? noteInstance = note.NoteType switch
        {
            Notes.NoteType.Tap => new TapDisplay
            {
                 Index = note.Index,
                 Name = $"tapInstance{angle.Index}T{note.Index}",
                 Margin = new(0, -1360, 0, 0)
             },
             ...
             _ => null
         };
    }
    ```
  依此计算下落时间：`var flootTime = 1410 / (double)(200 * (angle.Speed));`。
