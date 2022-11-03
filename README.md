# CatLibSample
记录：
1、首先创建一个空的URP工程，删除示例所带的ExampleAssets、Materials等示例所创建的文件夹；
2、(1)在工程Assets下创建一个CatLib目录;
  (2)导入CatLib For Unity:https://github.com/CatLib/CatLib.git
     a.将src目录下除了Example目录外，全部拷贝到Unity工程下的Assets/CatLib目录下;
     b.将Demo目录下的所有文件拷贝到工程下的Scripts目录(若无则创建一下)
  (3)导入CatLib Core:https://github.com/CatLib/Core.git
     a.用visual studio 打开CatLib.Core.sln解决方案，编译，注：确保还原NuGet资源包，如未，选中vs下的选项中关于NuGet中的选贤
     b.将生成好的dll(在src/CatLib.Core/bin目录下)拷贝到unity工程下的Assets/CatLib/Core下(Core目录需要手动创建一下)
  (4)导入部分CatLib Framework:https://github.com/CatLib/Framework.git
     a.这里只将Framework中的FileSystem及其依赖的相关部分集成到了工程中，放到Assets/Scripts/Framework下
     注：CatLib Framework依赖的CatLib Core和步骤(3)中仓库的Core不相同，缺少部分类支持，但实际上需要这部分的类，我发现在CatLib Core仓库的历史版本中找了，所以可以clone历史版本下来，整合进去，放到了Assets/Scripts/Framework下的Framework.Util目录
3、在unity工程下将Assets/Scenes/SampleScene改名成Luancher，并将ExampleAssets相关节点删除，新建一个Bootstrap节点，挂载脚本Main.cs

至此1-3步已经将CatLib整合进unity工程中去，后续如要对应的CatLib Framework的话，可进一步扩展

4、导入XLua，本工程导入的是版本是：lua54_v2.1.16_with_silicon_support
