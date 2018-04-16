#
# MySQL_Docker.ps1
#
$containerName = "pfsign_mysql"
switch($args)
{
    "up" { docker run -d --name $containerName -p 3306:3306 -e MYSQL_ROOT_PASSWORD=password mysql; break }
    "stop" { docker stop $containerName; break }
    "rm" { docker rm $containerName -v; break }
	"down" { docker rm $containerName -f -v; break }
    Default { echo "you can use: up, stop, rm, down" }
}