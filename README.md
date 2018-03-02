# 攀峰工作室签到系统

已完成所有功能性的开发[Demo](https://pfsigndev.azurewebsites.net/index.html)。

主要的几个如下：

- 使用Azure AD B2C作为认证服务，增加了账号以及权限限制。
- 使用`Pomelo.AspNetCore.TimedJob`，增加了定时强制清人的功能。
- 应用架构方面，放弃了Razor视图的写法，使用Js与后台API交互。
- 解决了历史遗留的时区问题。
