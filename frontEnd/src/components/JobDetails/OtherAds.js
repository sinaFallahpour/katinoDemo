import React from "react";
import { Link, useHistory } from 'react-router-dom';
import { Ad } from "../home";

export function OtherAds(props) {

  const history = useHistory();
  console.log(props.city)

  return (
    <React.Fragment>
      <h4 className="d-flex flex-row justify-content-between align-items-baseline ir-b c-dark text-right d-block fs-l smb-3">
        سایر آگهی ها

        <Link style={{ fontSize: "1rem" }} to="/Employee/newsSubscribtion" className="m-0 p-0 text-decoration-none d-flex flex-row justify-content-between align-items-center"
          onClick={(e) => {
            e.preventDefault();
            history.push("/Employee/newsSubscribtion", {
              feildOfActivity: props.feildOfActivity,
              jobTitle: props.jobTitle,
              typeOfCooperation: props.typeOfCooperation,
              city: props.city
            });
          }}>
          اطلاع‌رسانی از طریق ایمیل
          <i class="fa fa-envelope mr-1" aria-hidden="true"></i>
        </Link>
      </h4>

      <div className="bg-white sp-2 srounded-md">
        {!props.list ? (
          <div className="ir-r d-block text-right c-regular">
            در حال بارگیری
          </div>
        ) : (
          props.list.map((item) => (
            <div key={item.id} className="smb-2">
              <Ad
                adType={2}
                id={item.id}
                title={item.title}
                companyName={item.companyName}
                city={item.city}
                salary={item.salary}
                typeOfCooperation={item.typeOfCooperation}
                descriptionOfJob={item.descriptionOfJob}
                item={item}
                handleMarkOtherAdv={props.handleMarkOtherAdv}
              />
            </div>
          ))
        )}
      </div>
    </React.Fragment>
  );
}
