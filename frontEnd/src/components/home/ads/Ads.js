import React from "react";
import { Link } from "react-router-dom";
import { Immediately, Latest } from "./index";

export function Ads({ immediately, latest, handleMarkOtherAdv }) {

  console.log(handleMarkOtherAdv);

  return (
    <section className="container-fluid p-0 smy-10">
      <div className="p-0 m-0 d-flex flex-row flex-wrap justify-content-start align-items-stretch">
        
        <div className="col-xl-6 col-12 pl-1 " style={{marginTop:".1rem"}}>
          <h3 className="d-block text-right fs-l c-dark smb-2 ir-b">
            آگهی های <span className=" ir-b fs-m m-0 p-0">فوری</span>
          </h3>
          <div className=" bg-white p-3 rounded  smb-2 mb-lg-0">
            <Immediately
              immediately={immediately}
              handleMarkOtherAdv={handleMarkOtherAdv}
              status="immediate"
            />
          </div>
        </div>
        <div className="col-xl-6 col-12 pr-1">
          <h3 className="d-block text-right fs-l c-dark smb-2 ir-b">
            آخرین آگهی ها
          </h3>
          <div className=" bg-white p-3 rounded smb-2 mb-lg-0">
            <Latest
              hasMoreButton={true}
              latest={latest}
              handleMarkOtherAdv={handleMarkOtherAdv}
              status="latest"
            />
          </div>
        </div>
        
      </div>
      <div className="col-12 my-2 d-flex-flex-row justify-content-center w-100">
        
          <Link
            to="/Jobs"
            style={{maxWidth:"150px"}}
            className="btn btn-primary mx-auto srounded-sm ir-r d-flex justify-content-between align-item-center spy-1 spx-2 shadow-none"
          >
            سایر آگهی ها
            <i className="fas fa-chevron-left d-flex align-item-center"></i>
          </Link>
        
      </div>
     
    </section>
  );
}
