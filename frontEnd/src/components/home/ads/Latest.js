import React from "react";
import { Ad } from "./Ad";
import { Link } from "react-router-dom";

export function Latest({ latest, handleMarkOtherAdv, status, adType = 1 }) {
 
  if (!latest) return <> </>;
  return (
    <div className="row p-0 m-0">
      {latest.map((item) => (
        <div key={item.id} className="col-12 p-0 smb-1">
          <Ad
            adType={adType}
            id={item.id}
            title={item.title}
            companyName={item.companyName}
            city={item.city}
            salary={item.salary}
            type={item.typeOfCooperation}
            typeOfCooperation={item.typeOfCooperation}
            item={item}
            handleMarkOtherAdv={handleMarkOtherAdv}
            status={status}
          />
        </div>
      ))}

      
    </div>
  );
}
