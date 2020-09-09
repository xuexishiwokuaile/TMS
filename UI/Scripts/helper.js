//获取登陆状态
function GetLoginState() {
    sessionid = sessionStorage.getItem("loginId");
    if (sessionid != null) {
        return true;
    }
    else {
        return false;
    }
}