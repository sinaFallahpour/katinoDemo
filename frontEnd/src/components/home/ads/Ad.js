// import React, { Component } from "react";
// import { cooperationType, salary, findCities } from "../../../enums";
// import axios from "axios";
// import API_ADDRESS from "../../../API_ADDRESS";
// import { toast } from "react-toastify";
// import "react-toastify/dist/ReactToastify.css";

// export class Ad extends Component {
//   state = {
//     desc: "",
//     markAd: false,
//   };

//   componentDidMount() {
//     if (props.descriptionOfJob) {
//       let string = props.descriptionOfJob;
//       string = string.substr(0, 200);
//       setState({ desc: string });
//     }
//   }

//   adMarker = () => {
//     if (state.markAd === false) {
//       axios
//         .post(
//           API_ADDRESS + `Adver/MarkAdvder?adverId=${props.id}`,
//           {},
//           {
//             headers: {
//               Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
//             },
//           }
//         )
//         .then(() => {
//           setState({ markAd: true });
//           toast.success("با موفقیت نشان شد.");
//         })
//         .catch((err) => {
//           toast.warn("لطفا وارد شوید.");
//         });
//     } else {
//       axios
//         .post(API_ADDRESS + `Adver/UnMarkAdvder?adverId=${props.id}`, {
//           headers: {
//             Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
//           },
//         })
//         .then(() => setState({ markAd: false }))
//         .catch();
//     }
//   };

//   render() {
//     return (
//       <div className="card ad srounded-sm sp-2 text-decoration-none" style={{ backgroundImage: "url('/img/Untitled.png')" }}>
//         <header className="d-flex justify-content-between align-items-center smb-1">
//           <a
//             className="fs-m ir-b c-dark text-truncate"
//             href={`/JobDetails/${props.id}`}
//             dideo-checked="true"
//             style={{ textDecoration: "none" }}
//           >
//             {props.title}
//           </a>
//           <i
//             onClick={() => {
//               props.status === "latest" &&
//                 props?.handleMarkOtherAdv(props.id, "latest");
//               props.status === "immediate" &&
//                 props?.handleMarkOtherAdv(props.id, "immediate");
//               !props.status &&
//                 props?.handleMarkOtherAdv(props.id);
//             }}
//             className={`bookmarker-btn c-dark fs-l ${props.item?.isMarked === false ? "far" : "fas"
//               } fa-bookmark`}
//           ></i>
//         </header>

//         <a
//           className="card-body p-0"
//           href={`/JobDetails/${props.id}`}
//           style={{ textDecoration: "none" }}
//         >
//           <div className="detail smb-1">
//             <div className="row">
//               <div className="ir-r c-grey fs-m col-lg-12 mt-2" to="/">
//                 <i className="fas fa-building ml-2"></i>
//                 نام شرکت: <span
//                   style={{
//                     border: "1px solid #c4aaaa",
//                     borderRadius: "14px",
//                     display: "inline-block",
//                     width: "70%",
//                     textAlign: "center",
//                     color: "black",
//                     background: "#fffdfd",
//                   }}
//                 >
//                   {props.companyName}
//                 </span>
//               </div>

//               <div className="ir-r c-grey fs-m  col-lg-12 mt-2">
//                 <i className="fas fa-map-marker-alt ml-2"></i>
//                    شهر : {'\u00A0'} {'\u00A0'} {'\u00A0'} {'\u00A0'} <span
//                   style={{
//                     border: "1px solid #c4aaaa",
//                     borderRadius: "14px",
//                     display: "inline-block",
//                     width: "70%",
//                     textAlign: "center",
//                     color: "black",
//                     background: "#fffdfd",
//                   }}
//                 >
//                   {findCities(props.city)}
//                 </span>
//               </div>
//             </div>


//             <div className="row">
//               <div className="ir-r c-grey fs-m col-lg-12 mt-2" to="/">
//                 <i className="fas fa-building ml-2"></i>
//                 میزان حقوق: <span
//                   style={{
//                     border: "1px solid #c4aaaa",
//                     borderRadius: "14px",
//                     display: "inline-block",
//                     width: "66%",
//                     textAlign: "center",
//                     color: "black",
//                     background: "#fffdfd",
//                   }}

//                 >
//                   {`${salary(props.salary)} تومان`}
//                 </span>
//               </div>

//               <div className="ir-r c-grey fs-m col-lg-12 mt-2">
//                 <i className="fas fa-map-marker-alt ml-2"></i>
//                 نوع قرار داد: <span
//                   style={{
//                     border: "1px solid #c4aaaa",
//                     borderRadius: "14px",
//                     display: "inline-block",
//                     width: "70%",
//                     textAlign: "center",
//                     color: "black",
//                     background: "#fffdfd",
//                   }}
//                 >
//                   {cooperationType(
//                     props.typeOfCooperation
//                   )}
//                 </span>
//               </div>
//             </div>



//             {/* <div className="row">
//               <div>
//                 <i className="fas fa-map-marker-alt ml-2"></i>
//                 میزان حقوق:<span className="ir-r text-success fs-m sml-1">
//                   {`  ${salary(props.salary)} تومان `}
//                 </span>
//               </div>

//             </div>

//             <div>
//               <span className="ir-r c-grey fs-m ml-0">{` نوع قرار داد: ${cooperationType(
//                 props.typeOfCooperation
//               )} `}</span>
//             </div> */}
//           </div>

//           <p
//             className="d-block text-right ir-r fs-m mb-0 c-regular"
//             dangerouslySetInnerHTML={{ __html: `${state.desc}...` }}
//           ></p>
//         </a>
//       </div>
//     );
//   }
// }







import React, { Component, useEffect, useState } from "react";
import { cooperationType, salary, findCities } from "../../../enums";
import { Link, useHistory } from "react-router-dom"
import axios from "axios";
import jwt_decode from "jwt-decode";
import API_ADDRESS from "../../../API_ADDRESS";

import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./ad-style/style.css";

export const Ad = (props) => {

  const history = useHistory();

  const [state, setState] = useState({
    desc: "",
    markAd: false,
  });

  useEffect(() => {
    if (props.descriptionOfJob) {
      let string = props.descriptionOfJob;
      string = string.substr(0, 200);
      const state1 = { ...state };
      state1.desc = string;
      setState(state1);
    }
  }, [])

  const adMarker = () => {
    if (state.markAd === false) {
      axios
        .post(
          API_ADDRESS + `Adver/MarkAdvder?adverId=${props.id}`,
          {},
          {
            headers: {
              Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
            },
          }
        )
        .then(() => {
          const state1 = { ...state };
          state1.markAd = true
          setState(state1);
          toast.success("با موفقیت نشان شد.");
        })
        .catch((err) => {
          toast.warn("لطفا وارد شوید.");
        });
    } else {
      axios
        .post(API_ADDRESS + `Adver/UnMarkAdvder?adverId=${props.id}`, {
          headers: {
            Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
          },
        })
        .then(() => {
          const state1 = { ...state };
          state1.markAd = false
          setState(state1);
        })
        .catch();
    }
  };

  const showAD = (e, id) => {
    e.preventDefault();

    if (!localStorage.getItem("JWT")) {
      toast.error("برای مشاهده آگهی باید به عنوان کارجو وارد سایت شوید .")
    }
    else {
      const { role } = jwt_decode(localStorage.getItem("JWT"));
      console.log(role)
      if (role === "Employee") {
        history.push(`/JobDetails/${id}`);
      }
      else {
        toast.error("برای مشاهده آگهی باید به عنوان کارجو وارد سایت شوید .")
      }

    }

  }

  let gender = "";
  switch (props.item?.gender.toString()) {
    case "1":
      gender = "مهم نیست";
      break;
    case "2":
      gender = "مرد";
      break;
    case "3":
      gender = "زن";
      break;
    case "4":
      gender = "مهم نیست";
      break;
    default:
      gender = "مهم نیست";
      break;
  }
  let leastEdu = "";
  switch (props.item?.degreeOfEducation.toString()) {
    case "1":
      leastEdu = "مهم نیست";
      break;
    case "2":
      leastEdu = "دیپلم";
      break;
    case "3":
      leastEdu = "کاردانی";
      break;
    case "4":
      leastEdu = "کارشناسی";
      break;
    case "5":
      leastEdu = "کارشناسی ارشد";
      break;
    case "6":
      leastEdu = "دکترا";
      break;
    default:
      leastEdu = "مهم نیست";
      break;
  }
  let leastResume = "";
  switch (props.item?.workExperience.toString()) {
    case "1":
      leastResume = "مهم نیست";
      break;
    case "2":
      leastResume = "کمتر از 3 سال";
      break;
    case "3":
      leastResume = "بین 3 تا 7 سال";
      break;
    case "4":
      leastResume = "بیشتر از 7 سال";
      break;
    default:
      leastResume = "مهم نیست";
      break;
  }

  const styles = {
    "*": {
      color: props?.color ? props.color : "#273c85",
    },
  };

  return (
    <div styles={styles} style={{ backgroundColor: props?.backgroundColor ? props.backgroundColor : "#fff" }} className="ad-2 ir-b m-0 my-2 p-3 d-flex flex-column justify-content-start align-items-stretch">
      <div className="ad2-header m-0 p-0  d-flex flex-row justify-content-between align-items-baseline">
        <div onClick={e => {
          e.preventDefault();

          if (!localStorage.getItem("JWT")) {
            history.push("/Employee/Login?adverId=" + props?.id);
          }
          else {
            const { role } = jwt_decode(localStorage.getItem("JWT"));
            console.log(role)
            if (role === "Employee") {
              history.push(`/JobDetails/${props.id}`);
            }
            else if (role === "Employer") {
              toast.error("برای مشاهده آگهی باید به عنوان کارجو وارد سایت شوید .")
            }

          }
        }} style={{ cursor: "pointer" }} className=" ad2-title m-0 p-0 text-decoration-none">
          <code className="ml-2">#{props?.id}</code>
          {props.title}
          {props.item?.isImmediate === "فوری" && <small style={{ fontSize: ".8rem" }} className="m-0 p-2 px-2 mr-2 rounded-pill badge badge-danger ir-b">{props.item?.isImmediate}</small>}

        </div>

        <i onClick={() => {
          props.status === "latest" &&
            props?.handleMarkOtherAdv(props.id, "latest");
          props.status === "immediate" &&
            props?.handleMarkOtherAdv(props.id, "immediate");
          !props.status &&
            props?.handleMarkOtherAdv(props.id);
        }}
          className={`bookmarker-btn c-dark fs-l ${props.item?.isMarked === false ? "far" : "fas"
            } fa-bookmark`}
        ></i>

      </div>
      <Link

        onClick={e => {
          e.preventDefault();

          if (!localStorage.getItem("JWT")) {
            history.push("/Employee/Login" );
          }
          else {
            const { role } = jwt_decode(localStorage.getItem("JWT"));
            console.log(role)
            if (role === "Employee") {
              history.push(`/JobDetails/${props.id}`);
            }
            else if (role === "Employer") {
              toast.error("برای مشاهده آگهی باید به عنوان کارجو وارد سایت شوید .")
            }

          }
        }}

        // to={`/JobDetails/${props.id}`}

        className="ad2-body text-decoration-none mt-2 d-flex flex-row flex-wrap justify-content-between align-items-stretch">
        {props.adType === 1 &&
          <>

            <div className="ad2-feature col-xl-6 col-lg-4 col-sm-6 col-12 my-2 pl-xl-2 pl-lg-0 pl-sm-2 p-0  d-flex flex-row justify-content-start align-items-center">
              <div className=" ad2-feature__key col-6  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fa fa-file-contract fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1">نوع قرارداد: </span>
              </div>
              <div className="ad2-feature__value  m-0  col-6 text-right">
                {cooperationType(props.typeOfCooperation)}
              </div>
            </div>


            <div className="ad2-feature col-xl-6 col-lg-4 col-sm-6 col-12 my-2 p-0 pr-xl-2 pr-lg-0 pr-sm-2 pr-0  d-flex flex-row justify-content-start align-items-center">
              <div className=" ad2-feature__key col-sm-6 col-6  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fas fa-school fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1 "> حوزه فعالیت: </span>
              </div>
              <div className="ad2-feature__value m-0 col-sm-6 col-6 text-right">
                {props.item && props.item?.feildOfActivity}
              </div>
            </div>


            <div className="ad2-feature col-xl-6 col-lg-4 col-sm-6 col-12 my-2 pl-xl-2 pl-lg-0 pl-sm-2 p-0  d-flex flex-row justify-content-start align-items-center">
              <div className=" ad2-feature__key col-6  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fas fa-money-check-alt fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1">میزان حقوق: </span>
              </div>
              <div className="ad2-feature__value  m-0 col-6 text-right">
                {salary(props.salary)}
              </div>
            </div>

            <div className="ad2-feature col-xl-6 col-lg-4 col-sm-6 col-12 my-2 p-0 pr-xl-2 pr-lg-0 pr-sm-2 pr-0  d-flex flex-row justify-content-start align-items-center">
              <div className=" ad2-feature__key col-sm-6 col-6  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fas fa-school fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1 ">حداقل تحصیلات: </span>
              </div>
              <div className="ad2-feature__value m-0 col-sm-6 col-6 text-right">
                {props.item && <>{leastEdu}</>}
              </div>
            </div>

            <div className="ad2-feature col-xl-6 col-lg-4 col-sm-6 col-12 my-2 pl-xl-2 pl-lg-0 pr-lg-0 pl-sm-2 p-0  d-flex flex-row justify-content-start align-items-center">
              <div className=" ad2-feature__key col-6  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fas fa-venus-mars fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1">جنسیت: </span>
              </div>
              <div className="ad2-feature__value  m-0 col-6 text-right">
                {props.item && <>{gender}</>}
              </div>
            </div>

            <div className="ad2-feature col-xl-6 col-lg-4 col-sm-6 col-12 my-2 p-0 pr-xl-2 pr-lg-0 pr-sm-2 pr-0  d-flex flex-row justify-content-start align-items-baseline">
              <div className=" ad2-feature__key col-sm-6 col-6  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fas fa-file fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1 ">شهر: </span>
              </div>
              <div className="ad2-feature__value m-0 col-sm-6 col-6 text-right">
                {props.item && <>{props.item?.city}</>}
              </div>
            </div>


          </>
        }
        {props.adType === 2 &&
          <>

            <div className="ad2-feature col-lg-6 col-12 my-2   d-flex flex-row justify-content-start align-items-center">
              <div className=" ad2-feature__key col-sm-6 col-5  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fa fa-file-contract fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1">نوع قرارداد: </span>
              </div>
              <div className="ad2-feature__value  m-0  col-sm-6 col-7 text-right">
                {cooperationType(props.typeOfCooperation)}
              </div>
            </div>

            <div className="ad2-feature col-lg-6 col-12 my-2    d-flex flex-row justify-content-start align-items-center">
              <div className=" ad2-feature__key col-sm-6 col-5  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fa fa-tasks fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1">حوزه فعالیت: </span>
              </div>
              <div className="ad2-feature__value m-0 col-sm-6 col-7 text-right">
                {props.item && props.item?.feildOfActivity}
              </div>
            </div>

            <div className="ad2-feature col-lg-6 col-12 my-2   d-flex flex-row justify-content-start align-items-center">
              <div className=" ad2-feature__key col-sm-6 col-5  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fas fa-money-check-alt fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1">میزان حقوق: </span>
              </div>
              <div className="ad2-feature__value  m-0 col-sm-6 col-7 text-right">
                {salary(props.salary)}
              </div>
            </div>

            <div className="ad2-feature col-lg-6 col-12 my-2    d-flex flex-row justify-content-start align-items-center">
              <div className=" ad2-feature__key col-sm-6 col-5  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fas fa-school fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1 ">حداقل تحصیلات: </span>
              </div>
              <div className="ad2-feature__value m-0 col-sm-6 col-7 text-right">
                {props.item && <>{leastEdu}</>}
              </div>
            </div>

            <div className="ad2-feature col-lg-6 col-12 my-2   d-flex flex-row justify-content-start align-items-center">
              <div className=" ad2-feature__key col-sm-6 col-5  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fas fa-venus-mars fa-2x"></i>
                <span className="ad2-feature__key__name m-0  p-1">جنسیت: </span>
              </div>
              <div className="ad2-feature__value  m-0 col-sm-6 col-7 text-right">
                {props.item && <>{gender}</>}
              </div>
            </div>

            <div className="ad2-feature col-lg-6 col-12 my-2    d-flex flex-row justify-content-start align-items-baseline">
              <div className=" ad2-feature__key col-sm-6 col-5  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                <i className="ad2-feature__key__icon fas fa-file fa-2x"></i>
                <span className="ad2-feature__key__name m-0 mr-1 p-1 ">شهر: </span>
              </div>
              <div className="ad2-feature__value m-0 col-sm-6 col-7 text-right">
                {props.item && <>{props.item?.city}</>}
              </div>
            </div>

          </>
        }

      </Link>

    </div>
  );

}
