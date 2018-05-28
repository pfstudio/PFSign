# 攀峰签到系统

基于dev分支，移除了认证模块，并添加了Docker支持。

## 主要功能

1. 签到，签退，已经指定时间或学号的签到记录查询
2. 基于每个人、每天的签到时间统计
3. 定时清退，超时不签退惩罚

## API信息如下：

详细可参考[Postman Document](https://documenter.getpostman.com/view/2533705/RW8CGnYp)

### /api/Record

- GET /api/Record/
  查询签到记录，参数有人begin,end,studentId,skip,size
- /Record/SignIn POST
  签到，参数有studentId, name
- /Record/SignOut POST
  签退，参数有studentId


### /Report

- /Report GET
  查询本周内的每个人的签到时间统计
- /Report/{studentId} GET
  查询本周内某个人的每天签到时间

## 更新日志

### 2018/05/28

1. 尝试使用仓储模式，分离数据库逻辑
2. 尝试使用REST设计模式优化API
3. 在容器构建时写入时区信息 Asia/Shanghai
4. 数据库连接字符串的获取形式改为通过环境变量注入

### 2018/05/06

你未见过的船新版本，强行大幅度修改。

主要修改内容

1. 使用Filter和DataAnnotations, 提取了在签到签退时对参数以及状态的验证（就强行用，目测是坑
2. 命名空间还是改成PFSign（强行加上PFStudio真的难受，我的锅
3. 把对IQueryable对象的过滤改写为拓展方法（强行看着好看些
4. 修改了查询时的默认参数设置，当前Report API时间段默认为本周（从周一开始）
	> 唯一预期的修改。。。我到底干了什么。。一脸阿库娅