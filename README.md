# 攀峰签到系统

基于dev分支，移除了认证模块，并添加了Docker支持。

## 主要功能

1. 签到，签退，已经指定时间或学号的签到记录查询
2. 基于每个人、每天的签到时间统计
3. 定时清退，超时不签退惩罚

## API信息如下：

### /Record

- /Record/Query GET
  查询签到记录，参数有begin, end, studentId
- /Record/SignIn POST
  签到，参数有studentId, name
- /Record/SignOut POST
  签退，参数有studentId


### /Report

- /Report GET
  查询指定时间内的每个人的签到时间统计
- /Report/{studentId} GET
  查询指定时间内某个人的签到概览
- /Report/{studentId}/detail GET
  查询指定时间内某个人的每天签到时间
