import React, { useState } from "react";
import { useHistory, Link } from "react-router-dom";
// import { useForceUpdate, useForceUpdateWithCallback } from 'use-force-update-hook';
import axios from "axios";
import { toast } from "react-toastify";
import Swal from "sweetalert2";
import { MiniSpinner } from "../spinner/MiniSpinner";
import { AdverDetails } from "./AdverDetails";
import { adverStatus } from "../../enums";
import API_ADDRESS from "../../API_ADDRESS";

export function AdStatus(props) {
  // const forceUpdate = useForceUpdateWithCallback(() => {  });
  const [loading, setLoading] = useState(false);
  const [toggle, setToggle] = useState(false);
  const [adverId, setAdverId] = useState();

  const history = useHistory();

  let asignments = {
    status_1: 0,
    status_2: 0,
    status_3: 0,
    status_4: 0,
  };

  props.asignStatusWithCounts.map((item) => {
    switch (item.asingResomeStatus) {
      case 1:
        asignments.status_1 = item.count;
        break;

      case 2:
        asignments.status_2 = item.count;
        break;

      case 3:
        asignments.status_3 = item.count;
        break;

      case 4:
        asignments.status_4 = item.count;
        break;
    }
  });

  const activeAdverDraft = () => {
    setLoading(true);
    axios
      .post(
        API_ADDRESS + `Adver/AddAdverFromDraft?adverId=${props.id}`,
        {},
        {
          headers: {
            Authorization: `bearer ${localStorage.getItem("JWT")}`,
          },
        }
      )
      .then(() => {
        window.location.reload();
        Swal.fire({
          icon: "success",
          title: "آگهی با موفقیت فعال شد",
          showConfirmButton: false,
          timer: 1750,
        });
        setLoading(false);
      })
      .catch((err) => {
        err?.response?.data?.message.map((e) => {
          toast.error(e);
        });

        setLoading(false);
      });
  };

  function goto(event) {
    if (event.target.id !== "modalContaierOfAdver") {
      setToggle(false);
    }
  };

  document.body.addEventListener("click", goto);

  const SetImmediateAdver = () => {
    setLoading(true);
    axios
      .post(
        API_ADDRESS + `Adver/SetImmediateAdver?adverId=${props.id}`,
        {},
        {
          headers: {
            Authorization: `bearer ${localStorage.getItem("JWT")}`,
          },
        }
      )
      .then(() => {
        window.location.reload();
        Swal.fire({
          icon: "success",
          title: "آگهی با موفقیت فوری سازی شد",
          showConfirmButton: false,
          timer: 1750,
        });
        setLoading(false);
      })
      .catch((err) => {
        err?.response?.data?.message.map((e) => {
          toast.error(e);
        });

        setLoading(false);
      });
  };

  const addToStoryAdver = () => {
    setLoading(true);
    axios
      .post(
        API_ADDRESS + `Adver/StorySazAdver?id=${props.id}`,
        {},
        {
          headers: {
            Authorization: `bearer ${localStorage.getItem("JWT")}`,
          },
        }
      )
      .then(() => {
        window.location.reload();
        Swal.fire({
          icon: "success",
          title: "آگهی با موفقیت استوری شد",
          showConfirmButton: false,
          timer: 1750,
        });
        setLoading(false);
      })
      .catch((err) => {
        err?.response?.data?.message.map((e) => {
          toast.error(e);
        });

        setLoading(false);
      });
  };

  const handleVoidAdver = async () => {

    const url = "https://panel.katinojob.ir/api/Adver/EmployerDisableAdver?adverId=" + props.id;

    setLoading(true);

    await axios
      .get(url)
      .then(() => {
        toast.success("آگهی با موفقیت باطل شد .");
        window.location.reload();
        // forceUpdate();
      })
      .catch(ex => {
        if (ex.response) {
          toast.error(ex.response[0]);
        }
      });

    setLoading(false);
  };

  return (
    <>
      {loading && MiniSpinner()}
      {toggle && <AdverDetails adverId={adverId} />}
      <div className="card  srounded-sm sp-2">
        <div className="row">
          <header className="col-12 smb-2">
            <div className="d-lg-flex justify-content-lg-between align-items-lg-center">
              <div className="smb-2 mb-lg-0">
                <code
                  className="badge badge-danger"
                  style={{
                    height: "unset",
                    width: "unset",
                    lineHeight: "unset",
                    fontSize: "unset",
                  }}>
                  {props.id}#
                </code>

                {props.adverCreatationStatus === 3 ? (
                  <Link to={`/Employer/AdInfo/${props.id}`}>
                    <span className="ir-b c-dark"> {props.title} </span>
                  </Link>
                ) : (
                  <span>
                    <span className="ir-b c-dark"> {props.title} </span>
                  </span>
                )}
                {props.isImmediate === "فوری" && (
                  <span
                    className="c-danger ir-r smr-1 bg-body srounded-sm sp-05"
                    style={{ display: "inline-block" }}
                  >
                    آگهی فوری
                  </span>
                )}
                {props.item.isStorySaz === true && (
                  <span
                    className=" text-white ir-r smr-1 bg-primary srounded-sm sp-05"
                    style={{ display: "inline-block" }}
                  >
                    استوری شده
                  </span>
                )}
                {props.item.isActive === true && (
                  <span
                    className=" text-white ir-r smr-1 bg-danger srounded-sm sp-05"
                    style={{ display: "inline-block" }}
                  >
                    باطل شده
                  </span>
                )}

                <span
                  className="c-grey ir-r smr-1 bg-body srounded-sm sp-05"
                  style={{ display: "inline-block" }}
                >
                  {adverStatus(props.adverStatus)}
                </span>

                {props.adverStatus !== 2 && (
                  <span
                    className={`text-white  ir-r smr-1 srounded-sm sp-05 
                ${props.adverCreatationStatus === 1 && "bg-success"}
                ${props.adverCreatationStatus === 3 && "bg-success"}
                ${props.adverCreatationStatus === 2 && "bg-danger"}
                ${props.adverCreatationStatus === 4 && "bg-danger"}
                `}
                    style={{ display: "inline-block" }}
                  >
                    {props.adverCreatationStatus === 1 && "درحال بررسی"}
                    {props.adverCreatationStatus === 2 && "رد شده"}
                    {props.adverCreatationStatus === 3 && "پذیرفته شده"}
                    {props.adverCreatationStatus === 4 && "برگشت خورده"}
                  </span>
                )}
              </div>

              <div>
                <span
                  className="btn btn-light sml-1 ir-r"
                  onClick={() => {
                    setToggle(true);
                    setAdverId(props.id);
                  }}
                >
                  بیشتر
                </span>

                {props.adverCreatationStatus === 1 && (
                  <Link
                    to={`/Employer/editAdver?AdverId=${props.id}`}
                    className="btn btn-light sml-1 ir-r"
                  >
                    ویرایش
                  </Link>
                )}
                {props.adverCreatationStatus === 4 && (
                  <Link
                    to={`/Employer/editAdver?AdverId=${props.id}`}
                    className="btn btn-light sml-1 ir-r"
                  >
                    ویرایش
                  </Link>
                )}

                { props.item.isActive === false && 
                <button
                  onClick={handleVoidAdver}
                  className="text-white bg-danger btn btn-light sml-1 ir-r"
                >
                  باطل کردن
                </button>}

                {props.adverStatus === 2 && (
                  <button
                    onClick={activeAdverDraft}
                    className="text-white bg-success btn btn-light sml-1 ir-r"
                  >
                    فعال کردن
                  </button>
                )}

                {props.adverCreatationStatus === 3 &&
                  props.isImmediate !== "فوری" && (
                    <span
                      onClick={SetImmediateAdver}
                      className="text-white  ir-r smr-1 srounded-sm sp-05 p-2 bg-danger"
                      style={{ display: "inline-block", cursor: "pointer" }}
                    >
                      فوری سازی آگهی
                    </span>
                  )}

                { (props.adverCreatationStatus !== 2) && props.item.isStorySaz !== true && props.item.isActive === false  && (
                  <span
                    onClick={addToStoryAdver}
                    className="text-white  ir-r smr-1 srounded-sm sp-05 bg-primary p-2"
                    style={{ display: "inline-block", cursor: "pointer", fontSize: ".92rem" }}
                  >
                    استوری کردن آگهی
                  </span>
                )}
              </div>
            </div>

            <span className="ir-r c-regular smt-2">
              {props.adverCreatationStatus === 2 && props.adminDescription
                ? `پیام سیستم: ${props.adminDescription}`
                : ""}
              {props.adverCreatationStatus === 4 && props.adminDescription
                ? `پیام سیستم: ${props.adminDescription}`
                : ""}
            </span>
          </header>

          <div className="col-12 col-lg-6 smb-2 c-primary d-flex justify-content-start align-items-center">
            <span className="ir-r text-center d-block badge badge-warning fs-m p-0">
              {asignments.status_1}
            </span>
            <span className="ir-r smr-1 c-warning">در انتظار تعیین وضعیت</span>
          </div>

          <div className="col-12 col-lg-6 smb-2 c-primary d-flex justify-content-start align-items-center">
            <span className="ir-r text-center d-block badge badge-primary fs-m p-0">
              {asignments.status_3}
            </span>
            <span className="ir-r smr-1 c-primary">تایید برای مصاحبه</span>
          </div>

          <div className="col-12 col-lg-6 smb-2 c-primary d-flex justify-content-start align-items-center">
            <span className="ir-r text-center d-block badge badge-success fs-m p-0">
              {asignments.status_4}
            </span>
            <span
              className="ir-r smr-1 c-success"
              style={{
                color: "#50D86A",
              }}
            >
              استخدام شده
            </span>
          </div>



          <div className="col-12 col-lg-6 smb-2 c-primary d-flex justify-content-start align-items-center">
            <span className="ir-r text-center badge badge-danger fs-m p-0 d-block c-danger srounded-sm">
              {asignments.status_2}
            </span>
            <span className="ir-r smr-1 c-danger">رد شده</span>
          </div>

          <div class="col-12 col-lg-12 smb-2 c-primary d-flex justify-content-end align-items-center text-left ">
            <span class="ir-r smr-1 c-danger btn btn-success text-white" onClick={() => { history.push(`/Employer/AdInfo/${props.id}`) }}>نمایش رزومه های ارسالی</span>
          </div>
        </div>
      </div>
    </>
  );
}
