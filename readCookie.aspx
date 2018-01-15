<%@ Page Language="C#" AutoEventWireup="true" CodeFile="readCookie.aspx.cs" Inherits="readCookie" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0";charset="UTF-8" />

    <title>爱普锐健康报告</title>
    <link rel="stylesheet" href="css/style.css" />

    <script type="text/javascript">
        function nTabs(thisObj, Num) {
            if (thisObj.className == "active") return;
            var tabObj = thisObj.parentNode.id;
            var tabList = document.getElementById(tabObj).getElementsByTagName("li");
            if (Num != 2) {
                for (i = 0; i < tabList.length; i++) {
                    if (i == Num) {

                        thisObj.className = "active";

                        document.getElementById(tabObj + "_Content" + i).style.display = "block";
                    } else {

                        tabList[i].className = "actives";
                        ;

                        document.getElementById(tabObj + "_Content" + i).style.display = "none";
                    }
                }
            }
            else {
                var lj = document.getElementById("lianjiet").value;
                window.open(lj, "_blank")
            }

        }
    </script>
    <script type="text/javascript" src="js/jquery.min.js"></script>
   
</head>
<body>
 <header>
        <img class="logo" src="images/logo.png">

        <h3 runat="server" id="biaoti">郑州爱普锐智能高科股份有限公司</h3>
        <a href="share/modify.aspx">
            <img class="sharing" src="images/sharing.png">
        </a>
    </header>
    <!-- content -->
    <section class="content">
        <div class="cont-top">
            <figure>
                <img src="images/shengao.png">
                <figcaption style="font-size:medium;font-weight:200">身高<br/><label runat="server" style="font-size:26px;font-weight:700" id="heightt"></label></figcaption>
            </figure>
            <figure>
                <img src="images/tizhong.png">
                <figcaption style="font-size:medium;font-weight:200">体重<br/><label runat="server"  style="font-size:26px;font-weight:700" id="weightt"></label></figcaption>
            </figure>
            <figure>
                <img src="images/bmi.png"/>
                <figcaption style="font-size:medium;font-weight:200">健康指数<br/><label runat="server"  style="font-size:26px;font-weight:700" id="bmit"></label></figcaption>
            </figure>
            <div class="clear"></div>
            <h2 runat="server"  id="fenxit">体型分析正常请注意保持</h2>
        </div>
        <div class="cont-middle">
            <ul id="myTab1">
                <li class="active" onclick="nTabs(this,0);"><a style="font-size:16px" href="#">运动指导</a></li>
                <li class="actives" onclick="nTabs(this,1);"><a style="font-size:16px" href="#">饮食建议</a></li>
                <li class="actives" onclick="nTabs(this,2);"><a style="font-size:16px" href="#">历史数据</a></li>
            </ul>
            <div class="run-content" id="myTab1_Content0">
                <p runat="server"  id="ydzdt" style="font-size:14px"></p>
            </div>
            <div class="run-content none" id="myTab1_Content1">
                <p runat="server"  id="ysjyt" style="font-size:14px"></p>
            </div>
            <div class="run-content none" id="myTab1_Content2">

            </div>
        </div>
        <input runat="server" type="hidden" id="lianjiet" value="" />
        <div class="cont-bottom">
            <figure>
                <!--<img src="images/pic1.jpg">-->
                <!--轮播图-->
                <div class="slider">
                    <ul>
                        <li><img runat="server" src="blank" id="img1"></img></li>
                        <li><img runat="server" src="blank" id="img2"></img></li>
                        <li><img runat="server" src="blank" id="img3"></img></li>
                        <li><img runat="server" src="blank" id="img4"></img></li>
                    </ul>
                </div>
                <script type="text/javascript" src="js/yxMobileSlider.js"></script>
                <script>
                    $(".slider").yxMobileSlider({ width: 550, height: 380, during: 3000 })
                </script>


                <figcaption>
                    <h4 runat="server" id="titlet"></h4>

                    <p runat="server" style="width:380px;" id="contentt"></p>
                </figcaption>
            </figure>
        </div>
    </section>
    <!-- footer  -->
    <footer>
        <h3>广告投放热线：0371-67679112  15937106483</h3>
    </footer>

</body>
</html>
