#1-22
可以修改。
修改服务器代码就行，没有影响其他功能。
在下面的代码把200修改就行。
openroom.cpp
DBCB_DBGetVisitGroup1()
{
...
if (nRowsReal > 200)
{
...
nRowsReal = 200;
...
}
...
}
