# 工程迭代记录

## 创建空的URP工程(基于unity 2020.3.37)

* 创建一个空的URP工程，删除示例所带的ExampleAssets、Materials等示例所创建的文件夹。
* 将SampleScene改名Launcher，并将ExampleAssets相关节点删除，新建一个Bootstrap节点。

## 集成[CatLib](https://github.com/CatLib/CatLib)

1. 在工程Assets下创建一个CatLib目录。

2. 导入[CatLib For Unity](https://github.com/CatLib/CatLib.git)：
* 将src目录下除了Example目录外，全部拷贝到Unity工程下的Assets/CatLib目录下。
* 将Demo目录下的所有文件拷贝到工程下的Scripts目录(若无则创建一下)。

3. 导入CatLib Core:https://github.com/CatLib/Core.git
* 用visual studio 打开CatLib.Core.sln解决方案，编译，注：确保还原NuGet资源包，如未，选中vs下的选项中关于NuGet中的选贤
* 将生成好的dll(在src/CatLib.Core/bin目录下)拷贝到unity工程下的Assets/CatLib/Core下(Core目录需要手动创建一下)

4. 导入部分CatLib Framework:https://github.com/CatLib/Framework.git
* 这里只将Framework中的FileSystem及其依赖的相关部分集成到了工程中，放到Assets/Scripts/Framework下
注：CatLib Framework依赖的CatLib Core和步骤(3)中仓库的Core不相同，缺少部分类支持，但实际上需要这部分的类，我发现在CatLib Core仓库的历史版本中找了，所以可以clone历史版本下来，整合进去，放到了Assets/Scripts/Framework下的Framework.Util目录

## 挂载CatLib入口脚本

在unity工程下Bootstrap节点，挂载脚本Main.cs

## 至此1-3步已经将CatLib整合进unity工程中去，后续如要对应的CatLib Framework的话，可进一步扩展

## 导入[XLua](https://github.com/Tencent/xLua)

本工程导入的是版本是：lua54_v2.1.16_with_silicon_support

## 导入[YooAsset](https://github.com/tuyoogame/YooAsset)资源管理框架

本工程导入的是版本是：YooAsset 1.3.4


********************* 2022-11-18 *********************

## 导入[QFramework.UIKit](https://github.com/liangxiegame/UIKit)

导入QFramework.UIKit，增加OpenPanel异步支持和集成YooAsset资源加载

********************* 2022-11-24 *********************

## 导入[DoTween](http://dotween.demigiant.com/download.php)

 导入DoTween，及XLua对其的绑定