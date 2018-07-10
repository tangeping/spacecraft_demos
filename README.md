Spacecraft_unity3d_war
=============

## 本项目作为KBEngine服务端引擎的客户端演示而写，适用于Unity5.x

http://www.kbengine.org

## 官方论坛

	http://bbs.kbengine.org


## QQ交流群

	461368412


## KBE插件文档

	Assets\Plugins\kbengine_unity3d_plugins\README.md

## GO!

	1. 确保已经下载过KBEngine服务端引擎，如果没有下载请先下载
		下载服务端源码(KBEngine)：
			https://github.com/kbengine/kbengine/releases/latest

		编译(KBEngine)：
			http://www.kbengine.org/docs/build.html

		安装(KBEngine)：
			http://www.kbengine.org/docs/installation.html

	3. 拷贝服务端资产库"kbengine_spaceship_demos_assets"到服务端引擎根目录"kbengine/"之下，如下图：
![demo_configure](https://github.com/tangeping/spacecraft_demos/tree/master/img/screenshots/kebengine_demo_assert_copy.png)

	4. 通过服务端资产库生成KBE客户端插件（可选，默认已经带有一份，除非服务器有相关改动才需要再次生成）
		1: 双击运行 kbengine/kbengine_demos_asset/gensdk.bat
		2: 拷贝kbengine_unity3d_plugins到kbengine_unity3d_SpaceShip_demo\Assets\Plugins\            

    5.安装方法参考：https://github.com/kbengine/kbengine_unity3d_demo readme中描述的服务器kbengine_demos_assets直接用kbengine_spaceship_demos_assets替代即可
        

## 配置Demo(可选):
	改变登录IP地址与端口（注意：关于服务端端口部分参看http://www.kbengine.org/cn/docs/installation.html）:
![demo_configure](https://github.com/tangeping/spacecraft_demos/tree/master/img/screenshots/spacecraft_unity_config.png)

## 启动服务器:

	先开启服务端
		Windows:
			kbengine\kbengine_spaceship_demos_assets\start_server.bat

		Linux:
			kbengine\kbengine_spaceship_demos_assets\start_server.sh

	检查启动状态:
		如果启动成功将会在日志中找到"Components::process(): Found all the components!"。
		任何其他情况请在日志中搜索"ERROR"关键字，根据错误描述尝试解决。
		(More: http://www.kbengine.org/cn/docs/startup_shutdown.html)
        
## Spacecraft截图

![Spacecraft项目运行效果](https://github.com/tangeping/spacecraft_demos/tree/master/img/screenshots/login.png)
![Spacecraft项目运行效果](https://github.com/tangeping/spacecraft_demos/tree/master/img/screenshots/pick.png)
![Spacecraft项目运行效果](https://github.com/tangeping/spacecraft_demos/tree/master/img/screenshots/fight1.png)
![Spacecraft项目运行效果](https://github.com/tangeping/spacecraft_demos/tree/master/img/screenshots/fight2.jpg)
![Spacecraft项目运行效果](https://github.com/tangeping/spacecraft_demos/tree/master/img/screenshots/settle.png)












