import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { GetLandingPage } from "../../core/api/landing-page";
import "./footer.styles.css";

export function LandingFooter() {
  const [userStatus, setUserStatus] = useState();
  const [whatsApp, setWhatsApp] = useState("");
  const [Twitter, setTwitter] = useState("");
  const [telegram, setTelegram] = useState("");
  const [Linkedin, setLinkedin] = useState("");
  const [Instagram, setInstagram] = useState("");

  useEffect(() => {

    window.scrollTo(0, 0);

    const userInfo = localStorage.getItem("userInfo");
    userInfo === "Employee" ? setUserStatus(userInfo) : setUserStatus(null);

    GetLandingPage().then((res) =>
      res?.resul?.map((item) => {
        item.key === "whatsApp" && setWhatsApp(item.value);
        item.key === "Twitter" && setTwitter(item.value);
        item.key === "telegram" && setTelegram(item.value);
        item.key === "Linkedin" && setLinkedin(item.value);
        item.key === "Instagram" && setInstagram(item.value);
      })
    );
  }, []);

  return (
    <footer className="footer-parent">
      <div className="footer-holder ">
        <aside className="footer-container">
          <h3 className="fs-m c-dark ir-b smb-2">کارجویان</h3>

          <Link className="fs-m c-regular ir-r smb-1" to="/Jobs">
            آگهی های استخدام
          </Link>
          {!userStatus && (
            <Link className="fs-m c-regular ir-r smb-1" to="/Employee/Login/">
              ورود/ثبت نام کارجویان
            </Link>
          )}
          <Link className="fs-m c-regular ir-r smb-1" to="/">
            ایمیل های اطلاع رسانی
          </Link>

          <Link
            className="fs-m c-regular ir-r smb-1"
            to="/Employee/CreateResume"
          >
            رزومه ساز آنلاین
          </Link>
          <Link className="fs-m c-regular ir-r mb-0" to="/AllCompanies">
            آشنایی با شرکت ها
          </Link>
          <Link className="fs-m c-regular ir-r mb-0" to="/Blog?user=employee">
            وبلاگ کارجو
          </Link>
        </aside>

        {!userStatus && (
          <aside className="footer-container ">
            <h3 className="fs-m c-dark ir-b smb-2">کارفرمایان</h3>

            <Link className="fs-m c-regular ir-r smb-1" to="/Employer/CreateAd">
              درج آگهی استخدام
            </Link>
            <Link
              className="fs-m c-regular ir-r smb-1"
              to="/Employer/Dashboard"
            >
              ورود به بخش کارفرمایان
            </Link>
            <Link
              className="fs-m c-regular ir-r smb-1"
              to="/Employer/Dashboard/Plans"
            >
              تعرفه انتشار آگهی
            </Link>
            <Link className="fs-m c-regular ir-r smb-1"
              to="/Blog?user=employer">
              وبلاگ کارفرما
            </Link>
          </aside>
        )}
        <aside className="footer-container">
          <h3 className="fs-m c-dark ir-b smb-2">کاتینو</h3>

          <Link className="fs-m c-regular ir-r smb-1" to="/Policy">
            قوانین کاتینو
          </Link>
          <Link to="/onlinePaymentGuide" className="fs-m c-regular ir-r smb-1">
            راهنمای پرداخت آنلاین
          </Link>
          <Link className="fs-m c-regular ir-r smb-1" to="/Contact">
            تماس با کاتینو
          </Link>
          <Link className="fs-m c-regular ir-r smb-1" to="/AboutUS">
            درباره کاتینو
          </Link>
          <Link className="fs-m c-regular ir-r smb-1" to="/FrequentQuestion">
            سوالات متداول
          </Link>
          <Link className="fs-m c-regular ir-r smb-1" to="/EmployerTraining">
            راهنمای استفاده برای کارجویان
          </Link>
          <Link className="fs-m c-regular ir-r smb-1" to="/Blog?user=news">
            اخبار
          </Link>
          <Link id="weblog" className="fs-m c-regular ir-r smb-1"
            to="/Blog?user=katino">
            وبلاگ کاتینو
          </Link>

          <Link className="fs-m c-regular ir-r smb-1" to="/khadamatMa">
            خدمات ما
          </Link>

          <Link className="fs-m c-regular ir-r smb-1" to="/SharayetAkhzNamayande">
            شرایط اخذ نماینده
          </Link>
        </aside>

        <aside className="footer-container ">
          <h3 className="fs-m c-dark ir-b smb-2">شبکه های اجتماعی</h3>

          <div className="social w-100 d-flex justify-content-between align-item-center smb-4">
            <Link
              className="fs-l c-regular"
              target="_blank"
              to={{ pathname: Instagram }}
            >
              <i className="fab fa-instagram"></i>
            </Link>

            <Link
              className="fs-l c-regular"
              target="_blank"
              to={{ pathname: whatsApp }}
            >
              <i className="fab fa-whatsapp"></i>
            </Link>

            <Link
              className="fs-l c-regular"
              target="_blank"
              to={{ pathname: telegram }}
            >
              <i className="fab fa-telegram"></i>
            </Link>

            <Link
              className="fs-l c-regular"
              target="_blank"
              to={{ pathname: Linkedin }}
            >
              <i className="fab fa-linkedin-in"></i>
            </Link>

            <Link
              className="fs-l c-regular"
              target="_blank"
              to={{ pathname: localStorage.getItem("gmail") }}
            >
              <i className="fab fa-google"></i>
            </Link>
          </div>

          <div className="important-links d-flex justify-content-between align-items-center">
            <a
              className="il-item rounded-item bg-body srounded-md sp-1"
              href="#"
            >
              <img
                className="d-block w-100"
                src="/img/important-links/il-1.png"
                alt=""
              />
            </a>

            <a
              className="il-item rounded-item bg-body srounded-md sp-1"
              referrerpolicy="origin"
              target="_blank"
              href="https://trustseal.enamad.ir/?id=204188&amp;Code=1Yco3ryD2nqCP3qfn3sa"
            >
              <img
                referrerpolicy="origin"
                src="https://Trustseal.eNamad.ir/logo.aspx?id=204188&amp;Code=1Yco3ryD2nqCP3qfn3sa"
                alt=""
                style={{ cursor: "pointer" }}
                id="1Yco3ryD2nqCP3qfn3sa"
                className="d-block w-100"
              />
            </a>

            <a
              className="il-item rounded-item bg-body srounded-md sp-1"
              href="#"
            >
              <img
                className="d-block w-100"
                src="/img/important-links/il-3.png"
                alt=""
              />
            </a>
          </div>
        </aside>
      </div>

      <div className="row">
        <div className="col-12 border-top">
          <span className="d-block w-75 spy-2 ir-r c-regular text-center mx-auto">
            © 1400 - تمامی حقوق برای کاتینو محفوظ است.
          </span>
        </div>
      </div>
    </footer>
  );
}
