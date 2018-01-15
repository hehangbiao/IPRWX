using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class readCookie : System.Web.UI.Page
{
    public string stature = "0";
    public string weight = "0";
    public string jyys = "";
    public string BMI = "";
    public string result = "";
    public string OKWeight = "";
    public string OKRange = "";
    public string measuretime = "";
    public string debug = "";
    public string tip = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["device_sn"] != null)
        {
            string device_sn = Request.Cookies["device_sn"].Value;

            if (device_sn.Contains("tzzf"))
            {
                Response.Redirect("https://www.iprwx.com/fat/zhifang.aspx?openid=" + Request["openid"].ToString().Trim(), false);
                return;
            }
        }
        if (Request.Cookies["stature"] == null || Request.Cookies["weight"] == null || Request.Cookies["measuretime"] == null || Request.Cookies["deviceid"] == null || Request.Cookies["wcoaid"] == null) return;

        stature = Request.Cookies["stature"].Value.Trim();
        weight = Request.Cookies["weight"].Value.Trim();
        measuretime = Request.Cookies["measuretime"].Value;

        string sid = Request.Cookies["deviceid"].Value;
        string sid2 = Request.Cookies["wcoaid"].Value;

        if (Request.Cookies["test_time"] == null)
        {
            biaoti.InnerText = DateTime.Now.ToShortDateString();
        }
        else
        {
            biaoti.InnerText = Request.Cookies["test_time"].Value.ToString();
        }
        //string time = Request.Cookies["test_time"].Value;

        int devid = 0;
        int wcoid = 0;
        int merchantid = 0;

        //bool gotostatic = false;
        string headimage = string.Empty;
        string nike = string.Empty;
        string open = string.Empty;
        //Fan_ret ft = new Fan_ret();

        try
        {
            string openid = open = Request["openid"].ToString().Trim();

            //devid = Int32.Parse(sid);
            //wcoid = Int32.Parse(sid2);
            //ft = Fan_logic.execut(openid, devid, wcoid, measuretime);

            //headimage = ft.headimage;
            //tip = ft.tip;
            //nike = ft.nike;

            if (!string.IsNullOrEmpty(measuretime))
            {
                using (IPR_WX_PLATFORMEntities ent = new IPR_WX_PLATFORMEntities())
                {

                    var mt = ent.Measure_table.FirstOrDefault(m => m.com_time == measuretime);

                    if (!string.IsNullOrEmpty(sid) && !string.IsNullOrEmpty(sid2))
                    {
                        devid = Int32.Parse(sid);
                        wcoid = Int32.Parse(sid2);

                        if (ent.Fan_tmp_table.Any(f => f.fromuser == openid))
                        {
                            var fan = ent.Fan_tmp_table.FirstOrDefault(f => f.fromuser == openid);
                            DateTime dt = (DateTime)fan.day;

                            ent.Fan_tmp_table.Remove(fan);

                            var wc = ent.WCOA_table.FirstOrDefault(w => w.id == wcoid);
                            wc.fan_count++;
                            //wc.status = 1;
                            //ent.SaveChanges();

                            //积分计算模块

                            int merid = (int)ent.Device_table.Find(devid).approve;
                            if (merid != 0)
                            {
                                var mer = ent.Merchant_table.Find(merid);
                                mer.points++;
                                //ent.SaveChanges();

                            }

                            if (ent.Record_table.Any(rt => rt.deviceid == devid && rt.wcoaid == wcoid && rt.day == dt))
                            {
                                ent.Record_table.FirstOrDefault(rt => rt.deviceid == devid && rt.wcoaid == wcoid && rt.day == dt).count++;
                            }
                            else
                            {
                                Record_table rt = new Record_table();
                                rt.day = dt;
                                rt.count = 1;
                                rt.deviceid = devid;
                                rt.wcoaid = wcoid;
                                ent.Record_table.Add(rt);

                            }

                            //ent.SaveChanges();

                        }
                    }



                    if (mt == null || mt.fanid == null || mt.fanid == 0)
                    {

                        if (ent.Fan_table.Any(ft => ft.openid == openid))
                        {
                            var fan = ent.Fan_table.FirstOrDefault(ft => ft.openid == openid);

                            mt.fanid = fan.id;

                            ent.SaveChanges();

                            fan.wcoaid = mt.wcoaid;
                            fan.deviceid = mt.deviceid;

                            //ent.SaveChanges();
                        }
                        else
                        {
                            devid = Int32.Parse(sid);
                            wcoid = Int32.Parse(sid2);
                            Fan_table f = new Fan_table();
                            f.openid = openid;
                            f.wcoaid = wcoid;
                            f.deviceid = devid;
                            ent.Fan_table.Add(f);
                            ent.SaveChanges();

                            mt.fanid = f.id;

                            //ent.SaveChanges();

                        }

                    }

                    ent.SaveChanges();

                    if (ent.Fan_table.Any(ft => ft.openid == openid))
                    {
                        var fan = ent.Fan_table.FirstOrDefault(ft => ft.openid == openid);

                        headimage = fan.headimgurl;

                        nike = fan.nickname;

                    }


                    int ownerid = (int)ent.Device_table.First(d => d.id == mt.deviceid).ownerid;
                    merchantid = (int)ent.Device_table.First(d => d.id == mt.deviceid).approve;



                    if (ent.News_table.Any(t => t.ownerid == ownerid))
                    {
                        var ltt = ent.News_table.Where(t => t.ownerid == ownerid).ToList();

                        int iii = DateTime.Now.Second % ltt.Count();

                        tip = ltt.ElementAtOrDefault(iii).news;
                    }
                    else
                    {
                        var ltt = ent.News_table.Where(t => t.ownerid == 0).ToList();

                        int iii = DateTime.Now.Second % ltt.Count();

                        tip = ltt.ElementAtOrDefault(iii).news;
                    }

                    decimal s = (decimal.Parse(stature) / 100);
                    s = s * s;
                    //OKWeight = (s * 22).ToString("0.0kg");
                    //OKRange = (s * (decimal)18.5).ToString("0.0") + "-" + (s * (decimal)23.9).ToString("0.0kg");


                    double B = 0;
                    if (s > 0)
                        B = (double)(decimal.Parse(weight) / s);
                    BMI = B.ToString("0.0").Trim();

                    int health = 0;

                    if (B < 18.5)
                    {
                        result = "偏瘦";
                        //瘦
                        health = -1;
                    }
                    else if (B >= 25)
                    {
                        result = "偏胖";
                        health = 1;
                    }
                    else
                    {
                        result = "正常";
                        health = 0;
                    }

                    //获取饮食建议
                    var lt = ent.Tip_table.Where(t => t.type == health).ToList();

                    int i = DateTime.Now.Second % lt.Count();

                    jyys = lt.ElementAtOrDefault(i).detail;

                    string p1 = null;
                    string p2 = null;
                    string p3 = null;
                    string p4 = null;

                    string title = null;
                    string content = null;

                    if (merchantid != 0)
                    {
                        if (ent.Promote_table.Any(p => p.merchantid == merchantid && p.promote_content != null))
                        {
                            var promote = ent.Promote_table.FirstOrDefault(p => p.merchantid == merchantid);

                            p1 = promote.picture_one;
                            p2 = promote.picture_two;
                            p3 = promote.picture_three;
                            p4 = promote.picture_four;

                            title = promote.title;
                            content = promote.promote_content;
                        }

                    }


                    if (p1 == null)
                    {
                        p1 = "https://www.iprwx.com/img/default/1.jpg";
                        p2 = "https://www.iprwx.com/img/default/2.jpg";
                        p3 = "https://www.iprwx.com/img/default/3.jpg";
                        p4 = "https://www.iprwx.com/img/default/4.jpg";

                        title = "商家优惠活动";
                        content = "优惠活动说明区";
                    }

                    double heigh = double.Parse(stature);
                    double weigh = double.Parse(weight);



                    HttpCookie cookie = new HttpCookie("ipr_height");
                    cookie.Value = String.Format("{0:F1}", heigh);
                    cookie.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    HttpCookie cookie1 = new HttpCookie("ipr_weight");
                    cookie1.Value = String.Format("{0:F1}", weight);
                    cookie1.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.Cookies.Add(cookie1);

                    HttpCookie cookie2 = new HttpCookie("ipr_headimage");
                    cookie2.Value = string.IsNullOrEmpty(headimage) ? " " : headimage;
                    cookie2.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.Cookies.Add(cookie2);

                    HttpCookie cookie3 = new HttpCookie("ipr_nike");
                    cookie3.Value = string.IsNullOrEmpty(nike) ? " " : nike;
                    cookie3.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.Cookies.Add(cookie3);


                    //Session["ipr_height"] = String.Format("{0:F1}", heigh);
                    //Session["ipr_weight"] = String.Format("{0:F1}", weight);
                    //Session["ipr_headimage"] = string.IsNullOrEmpty(headimage) ? " " : headimage;
                    //Session["ipr_nike"] = string.IsNullOrEmpty(nike) ? " " : nike;
                    //Session["ipr_date"] = DateTime.Now.ToString("D");

                    //string url = "https://www.iprwx.com/jiankangbaogao.html?height=" + String.Format("{0:F}", heigh) + "&weight=" + String.Format("{0:F}", weight) + "&BMI=" + BMI + "&fenxi=您的体型分析结果：" + result + "&yundongzhidao=" + System.Web.HttpUtility.UrlDecode(tip, System.Text.Encoding.Default) + "&yinshijianyi=" + System.Web.HttpUtility.UrlDecode(jyys, System.Text.Encoding.Default)
                    //    + "&image1=" + p1 + "&image2=" + p2 + "&image3=" + p3 + "&image4=" + p4 + "&title=" + title + "&content=" + System.Web.HttpUtility.UrlDecode(content, System.Text.Encoding.Default) + "&lishishuju=https://www.iprwx.com/mymeasure.aspx&openid=" + openid;

                    //Response.Redirect(url,false);

                    heightt.InnerText = String.Format("{0:F1}", heigh);// +" cm";
                    weightt.InnerText = String.Format("{0:F1}", weight);// +" kg";
                    bmit.InnerText = BMI;

                    fenxit.InnerText = "您的体型：" + result;

                    ydzdt.InnerText = tip;
                    ysjyt.InnerText = jyys;
                    lianjiet.Value = "https://www.iprwx.com/mymeasure.aspx?openid=" + openid;

                    img1.Src = p1;
                    img2.Src = p2;
                    img3.Src = p3;
                    img4.Src = p4;

                    titlet.InnerText = title;
                    contentt.InnerText = content;

                }
            }
        }
        catch (Exception ex)
        {
            LogHelper.WriteLog(typeof(readCookie), ex.Message);
        }
    }
}