# Arstive
音乐游戏**Arstive**原型机，非游戏引擎开发，
运行于Windows平台，依赖.Net8.0框架，以
Windows Presentation Foundation为技术栈
进行开发。

## 简介
Arstive是以几何为特色的创新性下落式音游，判定
依靠异形判定线“判定角”来进行，包含三种下落式音符
Tap、Hold与Drag，以及自由音符Move，该音符给出两个按键，玩家应该在限定时间内由头部按键以随意路径滑动至尾部按键。

## 开发流程
本音游圆形基于WPF开发，其底层运用DirectX引擎进行相应渲染。使用多线程技术管理其中的每个判定角，
