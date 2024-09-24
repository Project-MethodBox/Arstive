# Arstive
音乐游戏**Arstive**原型机，非游戏引擎开发，
运行于Windows平台，依赖.Net8.0框架，以
Windows Presentation Foundation为技术栈
进行开发。<br/>
**这是音乐游戏Arstive的技术文档，包含WPF构造音游的一般思路和方法，供能力较为成熟的.Net开发者阅读。**

## 简介
Arstive是以几何为特色的平面式创新下落音游，判定依靠异形判定线“判定角”来进行，包含三种下落式音符Tap、Hold与Drag，以及一种自由音符（目前）Flick，该音符给出两个按键，玩家应该在限定时间内由头部按键以随意路径滑动至尾部按键。<br/>
开发基于传统的三层模型，数据使用Json格式存储，但是方便起见直接将读取方法写在Chart加载类里导致了DAL和BLL产生了部分合并，UI层使用MVC模型，主窗口分数等信息直接以数据绑定（单向）的方式绑定在相应静态类上。<br/>
动画使用WPF动画框架，Note仅有线性下落动画，判定角支持移动，旋转，透明度动画，判定角这些动画均支持缓动函数，由Easing结构定义。这些缓动来自于WPF动画系统，包括BackEase、BounceEase、CircleEase、CubicEase、ElasticEase、ExponentialEase、PowerEase、QuadraticEase、QuinticEase、SineEase等。<br/>
判定角每个角Angle均绑定一个键盘按键Key，满足唯一映射关系f:Angle→Key，也就是说，每个角只能绑定一个键，同一个键可以绑定到多个角上。当Tap音符接近指定角时，需要打击角绑定的按键，当Drag音符接近相应角时，只需在接触时按住需要打击角绑定的按键即可，Hold音符最为复杂，需要在其存在时打击一直按住按键直至结束。

## 运行原理
本音游原型基于WPF开发，其底层运用DirectX引擎进行相应渲染。
<br/>
首先，游戏读取谱面数据体，该数据体包含基本信息、判定线列表、自由Note列表，而下落式的三种Note绑定在判定角中，在数据体表现为判定角的子级。之后，游戏提交谱面数据体并开始主循环。<br/>
主循环使用多线程技术进行管理，其中n个线程管理其中的每个判定角进行下落式音符的判定，1个线程进行计时器的维护，1个线程用于自由音符的判定，1个线程进行音频的播放，以上线程在运行之前均使用一个Barrier阻塞，当所有线程均加载完毕后，Barrier被释放，游戏开始。<br/>
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
  有时候，第一个Note从创建到判定的时间差小于音乐的总播放时间（i.e. 某Tap被设置为在500ms时判定，而下落时间为2000ms），这时就会导致严重的偏差，这时应该使计数时间从-1500开始，-1500到0ms这段时间称为缓冲时间，这段时间内的线程上下文称之为缓冲池。一般地，应计算最初几个Note的差并给予Tick初始负值。
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
  我们知道，下落式音符是作为判定角的子级创建的，定位使用判定角坐标（JP），而判定角和自由音符是直接根据游戏窗体创建的，定位使用全局坐标（GP）。GP系统以左上角为原点(0,0)，右下角为最大坐标(w,h)，其中w和h代表窗口的长度的宽度。下落式音游通常拥有轨道的概念，事实上，判定角也是指定长度的不可见轨道，其长度为以w和h为直角边三角形的斜边长与Note的高度之和d。这样可以避免Note以可视化的形式创建造成灾难性的观感。则易知Note的初始位置应该为JP系统下为(0,-d,0,0)的外边距坐标，同时，该方案可以判定角移动时，确保Note下落的稳定，隔离了GP和JP系统，保证读谱的舒适度 。
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
- **动画实现**<br/>
  在游戏中，一切下落和移动行为均使用ThicknessAnimation实现，改变的是相应元素的外边距，而非位置坐标。在Note下落中，首先计算下落时间，如果下落时间小于等于判定时间与当前时间之差，就创建Note并播放下落动画。
    ```
    // Create double animation
    var startPosition = -1360;
    var endPosition = -320;
    if (note.GetType() == typeof(Notes.Hold))
    {
        startPosition -= (((Notes.Hold)note).EndTime - note.HitTime) *
                                    (200 * (angle.Speed)) / 1000;
        endPosition = -0;
    }
    var downAnimation = new ThicknessAnimation(new(0, startPosition, 0, 0),new(-0, endPosition, 0, 0),new Duration(TimeSpan.FromMilliseconds(actualFlootTime * 1000)))
    {
        AutoReverse = false
    };
    // Binding animation via story board
    var storyBoard = new Storyboard();
    storyBoard.Children.Add(downAnimation);
    Storyboard.SetTargetName(downAnimation, noteInstance.Name);
    Storyboard.SetTargetProperty(downAnimation,new(FrameworkElement.MarginProperty));
    ```
  判定角移动动画同理，判定角旋转旋转动画使用DoubleAnimation，该动画以线性方式修改判定角的RotateAngle依赖属性，该属性通过RenderTransform绑定在判定角最外层的Grid面板上。
    ```
    <Grid Visibility="{Binding AngleVisibility}">
        <Grid.RenderTransform>
            <TransformGroup>
                <RotateTransform Angle="{Binding RotateAngle}" CenterX="70" CenterY="1500" />
            </TransformGroup>
        </Grid.RenderTransform>
    ...
    ```
  据此直接创建动画即可。
    ```
    var rotateAnimation = new DoubleAnimation(angle.RotateAngle, rotateEvent.EndAngle,  rotateEvent.Duration)
    {
        AutoReverse = false
    };
    ```
- **触控系统**<br/>
  音乐游戏做主要的任务就是判定，判定需要获知按键的状态。触控系统接受一个Key作为参数，这个Key就是相应判定角绑定的按键，并维护两个字段tap与pressed，tap表明指定按键在当前10ms周期内被打击，pressed表面当前按键在当前10ms周期内处于按下状态。
  触控系统嵌于判定线程内，首先检查是否按下了绑定键，并且变量tap为假。如果这两个条件同时满足，将tap设置为真。这表示按键被首次按下。接下来，检查是否松开了绑定键并且变量pressed为真。如果这两个条件同时满足，将pressed设置为假，这表示按键被释放。最后，检查是否松开了绑定键并且变量tap为真。如果这个条件满足，将tap设置为假。
    ```
    if (Keyboard.IsKeyDown(angle.BindingKey) && !tap)
    {
        tap = !pressed;
        pressed = true;
    }
    else if (Keyboard.IsKeyUp(angle.BindingKey) && pressed)
    {
        pressed = false;
    }
    if (Keyboard.IsKeyUp(angle.BindingKey) && tap)
        tap = false;
    ```
  运用这个触控输入系统，我们可以完成各Note的判定。**Tap**的判定逻辑为：Tap处于判定区间内且tap为true；**Drag**的判定逻辑为：Drag处于判定区间内且pressed为true；**Hold**的判定逻辑为：Hold头部于判定区间内且tap为true，并且要求Hold的整个判定区间内pressed始终为true；**Flick**的判定逻辑为：Flick头部于弱判定区间内且头部按键tap为true，并且要求Flick的整个判定区间内任意pressed始终为true；同时，Flick尾部处于强判定区间内且尾部按键tap为true。弱判定区间是Flick头部使用的判定区间，由于Flick属于直接出现的自由音符，所以提供一个更大更弱的判定区间供用户反应，保证良好的游玩体验，强判定区间就是标准判定区间。
- **持续音符**<br/>
  在音游中，出了类似Tap和Drag这类瞬间判定完成的音符外，还有像本音游中Hold和Flick这样的持续判定音符。为突出表现，下落式持续判定音符必定包含单调且可以无限延长的尾部，而本音游中自由音符也拥有类似尾部的进度条。由于本游戏使用异形判定线，在下落方面持续音符也与瞬时音符存在一些不同。下面来详细讨论这些问题。<br/>
  **音符包含一个与判定角卡和的头部和单调的尾部。**Hold的头部与普通的Tap大致相似，但拖有一条白色的尾部，该尾部的RGB值为#FFFFFF（背景的反色），且Alpha通道存在由1至0的线性渐变。
    ```
    <Grid>
        <StackPanel>
            <Rectangle
                Width="70"
                Height="{Binding Length, Converter={StaticResource LengthConverter}}"
                d:Height="280">
                <Rectangle.Fill>
                    <LinearGradientBrush StartPoint="0.5,0" EndPoint="0.5,1">
                        <GradientStop Color="#4CFFFFFF" />
                        <GradientStop Offset="1" Color="#FFFFFFFF" />
                    </LinearGradientBrush>
                </Rectangle.Fill>
            </Rectangle>
            <Polygon
                DockPanel.Dock="Bottom"
                Fill="White"
                Points="25,0 115,0 70,40" />
        </StackPanel>
    </Grid>
    ```
  长度计算方面，很明显长度等于流速乘判定时间差（判定技术时间减判定开始时间），最后减去头部的长度，即`((((Notes.Hold)note).EndTime - ((Notes.Hold)note).HitTime) * (200 * (angle.Speed)))/1000 + 40 - 50`，其中常数项50是Hold头部长度，而40是判定角下侧绑定按键显示框的长度。
  **异形判定线对于Hold的判定。**Tap和Drag只需落到角上即可，而Hold需要落到轨道的最后侧，也就是需要加上一个判定角下侧绑定按键显示框的长度，但是判定仍需要在刚抵达角时进行头部击打判定。
    ```
    if (note.GetType() == typeof(Notes.Hold))
    {
        startPosition -= (((Notes.Hold)note).EndTime - note.HitTime) * (200 * (angle.Speed)) / 1000;
        endPosition = -0;
    }
    ```
    **Flick的固定时长设计。**Flick的长度可以任意改变，但总是以2s的时间完成整个过程，速度由动画系统自动计算。
    ```
    <UserControl.Triggers>
        <EventTrigger RoutedEvent="Loaded">
            <BeginStoryboard>
                <Storyboard>
                    <DoubleAnimation
                        AutoReverse="False"
                        Storyboard.TargetName="_this"
                        Storyboard.TargetProperty="Opacity"
                        From="0"
                        To="1"
                        Duration="0:0:0.5" />
                </Storyboard>
            </BeginStoryboard>
        </EventTrigger>
    </UserControl.Triggers>
    ```
## 版权条例
Atstive-Originally在`AGPL`许可证下开源，直接使用该项目必须遵守GNU Affero General Public License之一切规定与条例，AGPL要求使用本项目的所有项目必须公开提供其源代码。特别地，本项目旨在为WPF开发音乐游戏提供解决方案和一般方法，如果仅使用本项目内部分内容和解决方案，可以在向arab@methodbox.top发送邮件申请后，以更加宽松的MIT许可证之方式使用本项目部分内容。

## 贡献者
**策划**：@Lorkea-x<br/>
**代码**：@ArabidopsisDev
