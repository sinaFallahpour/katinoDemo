import React, { useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import axios from "axios"
import API_ADDRESS from "../API_ADDRESS"
import queryString from "query-string";
import Cookies from 'js-cookie'



export function Security(props) {
  const { username, key, role } = useParams();
  const [adverId, set_adverId] = useState(null);
  const [calledOnce, set_calledOnce] = useState(false);

  useEffect(() => {
    // console.log(props);

    console.log(queryString.parse(window.location.search))

  }, []);

  if (!calledOnce) {
    axios
      .get(
        `https://panel.katinojob.ir/api/Account/AdminForceLogin?phoneNumber=${username}&verificationCode=${key}&role=${role}`
      )
      .then(async (res) => {

        console.log(res)
        set_calledOnce(true);

        if (window.localStorage.getItem("JWT") !== null) {
          await window.localStorage.removeItem("JWT")
          await window.localStorage.removeItem("userInfo")
          await window.localStorage.setItem("JWT", res.data.resul.toke)
          await window.localStorage.setItem("userInfo", role)
        } else {
          await window.localStorage.setItem("JWT", res.data.resul.toke)
          await window.localStorage.setItem("userInfo", role)
        }



        const adID = queryString.parse(window.location.search)?.adverId;
        if (role == "employer") {
           Cookies.set('companyPersianName', res.data.resul.companyPersianName, { expires: 7 / 24 })
          Cookies.set('companyEngName', res.data.resul.companyEngName, { expires: 7 / 24 })
        }

        if (adID) {

          window.history.replaceState(null, null, "/JobDetails/" + adID);
          window.location.reload();

        }
        else if (role === "Employee") {

          window.history.replaceState(null, null, "/Employee/Jobs");
          window.location.reload();
        }
        else if (role === "Employer") {
          window.location.href = "/Employer/Dashboard"
          // window.history.replaceState(null, null, "/Employer/Dashboard");
          // window.location.reload();
        }

      })
      .catch(() => (window.location.href = "/Employee/Login"))
  }


  return <div></div>
}
