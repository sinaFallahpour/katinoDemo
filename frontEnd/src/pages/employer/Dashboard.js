import React, { Component, useEffect, useState } from "react";
import { Link, useHistory } from "react-router-dom";
import { UserAds } from "../../components/employerPanel";

import agent from "../../core/agent";
import { MiniSpinner } from "../../components/spinner/MiniSpinner";
import { useDebounce } from "../../core/customHook/debounce.hook";
import API_ADDRESS from "../../API_ADDRESS";
import axios from "axios";
import Cookies from 'js-cookie'


export const Dashboard = () => {
  const [userAds, setUserAds] = useState();
  const [marked, setMarked] = useState(0);
  const [searchTerm, setSearchTerm] = useState();
  const [loading, setLoading] = useState(false);
  const debouncedSearchTerm = useDebounce(searchTerm, 500);
  const history = useHistory();

  useEffect(() => {
    setLoading(true);
    let params = new URLSearchParams(window.location.search);
    const adverStatus = params.get("adverStatus");
    setMarked(parseInt(adverStatus));

    const fetcData = async () => {
      if (adverStatus) {
        const { data } = await agent.Adver.GetAllAdverByStatusForCurrectUser(
          adverStatus
        );
        console.log(data);
        setUserAds(data.resul);
        setLoading(false);
        return;
      }
      const { data } = await agent.Adver.getAllAdverForCurrectUser();
      setUserAds(data.resul);
      setLoading(false);

      return;
    };

    fetcData();




    axios
      .get(API_ADDRESS + "Account/GetEmployerInfo", {
        headers: { Authorization: `bearer ${localStorage.getItem("JWT")}` },
      })
      .then((res) => {
        // console.log(res.data.resul)
        // this.setState({
        //   userInfo: {
        //     companyPersianName: res.data.resul.companyPersianName,
        //     companyEngName: res.data.resul.companyEngName,
        //     name: res.data.resul.fullName,
        //     img: res.data.resul.image,
        //   },
        if (res.data.resul.companyPersianName && res.data.resul.companyPersianName) {

          Cookies.set('companyPersianName', res.data.resul.companyPersianName, { expires: 7 / 24 })
          Cookies.set('companyEngName', res.data.resul.companyEngName, { expires: 7 / 24 })
        }
        else {
          history.push('/Employer/EditProfile');
          // history.pushState(null, '/Employer/EditProfile');
        }
      })


  }, []);

  // useEffect(() => {

  //   setLoading(true);
  //   let params = new URLSearchParams(window.location.search);
  //   const adverStatus = params.get("adverStatus");
  //   setMarked(parseInt(adverStatus));

  //   const fetcData = async () => {
  //     if (adverStatus) {
  //       const { data } = await agent.Adver.GetAllAdverByStatusForCurrectUser(
  //         adverStatus
  //       );
  //       console.log(data);
  //       setUserAds(data.resul);
  //       setLoading(false);
  //       return;
  //     }
  //     const { data } = await agent.Adver.getAllAdverForCurrectUser();
  //     setUserAds(data.resul);
  //     setLoading(false);

  //     return;
  //   };

  //   fetcData();

  // }, [window.location.search]);

  // // search for adver
  // useEffect(() => {
  //   setLoading(true);

  //   const fetcData = async () => {
  //     if (debouncedSearchTerm) {
  //       const { data } = await agent.Adver.SearchAdverForCurrectUser(debouncedSearchTerm);
  //       setUserAds(data.resul);
  //       setLoading(false);
  //     } else {
  //       const { data } = await agent.Adver.getAllAdverForCurrectUser();
  //       setUserAds(data.resul);
  //       setLoading(false);
  //     }
  //   };

  //   fetcData();
  // }, [debouncedSearchTerm]);

  return (
    <>
      {loading && <MiniSpinner />}
      <section className="dash-employer container-fluid spx-2 smt-10 spx-lg-10">
        <div className="row">
          <div className="bg-white srounded-md sshadow w-100 sp-2">
            <nav className="navbar navbar-expand-lg pr-0 py-0">
              <div>
                <ul className="navbar-nav ml-auto">
                  <li className="nav-item smr-lg-4">
                    <Link
                      to="/Employer/Dashboard"
                      className={`
                      ${!marked ? "c-primary" : "c-grey"
                        } nav-link position-relative  ir-r fs-m p-0 smb-1 mb-lg-0
                      `}
                    >
                      همه آگهی ها
                    </Link>
                  </li>

                  <li className="nav-item smr-lg-4">
                    <Link
                      to="/Employer/Dashboard?adverStatus=1"
                      className={`
                      ${marked === 1 ? "c-primary " : "c-grey"
                        } nav-link position-relative  ir-r fs-m p-0 smb-1 mb-lg-0
                      `}
                    >
                      فعال
                    </Link>
                  </li>

                  <li className="nav-item smr-lg-4">
                    <Link
                      to="/Employer/Dashboard?adverStatus=2"
                      className={`
                      ${marked === 2 ? "c-primary " : "c-grey"
                        } nav-link position-relative  ir-r fs-m p-0 smb-1 mb-lg-0
                      `}
                    >
                      پیش نویس
                    </Link>
                  </li>

                  <li className="nav-item smr-lg-4">
                    <Link
                      to="/Employer/Dashboard?adverStatus=6"
                      className={`
                      ${marked === 6 ? "c-primary " : "c-grey"
                        } nav-link position-relative  ir-r fs-m p-0 smb-1 mb-lg-0
                      `}
                      style={{ background: " red !important" }}
                    >
                      منقضی شده
                    </Link>
                  </li>

                  <li className="nav-item smr-lg-4">
                    <Link
                      to="/Employer/Dashboard?adverStatus=8"
                      className={`
                      ${marked === 8 ? "c-primary " : "c-grey"
                        } nav-link position-relative  ir-r fs-m p-0 smb-1 mb-lg-0
                      `}
                      style={{ background: " red !important" }}
                    >
                      باطل شده
                    </Link>
                  </li>

                  <li className="nav-item smr-lg-4">
                    <Link
                      to="/Employer/Dashboard?adverStatus=7"
                      className={`
                      ${marked === 7 ? "c-primary " : "c-grey"
                        } nav-link position-relative  ir-r fs-m p-0 smb-1 mb-lg-0
                      `}
                      style={{ background: " red !important" }}
                    >
                      رد شده
                    </Link>
                  </li>


                </ul>
              </div>
            </nav>
          </div>
        </div>

        <div className="row smt-3">
          <div className="bg-white srounded-md sshadow sp-2 w-100">
            <div className="col-12">
              <div className="form-group">
                <input
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="ir-r form-control srounded-sm shadow-none"
                  placeholder="جستجو در آگهی ها"
                />
              </div>
            </div>

            <hr className="smy-2" />

            <UserAds ads={userAds} />
          </div>
        </div>
      </section>
    </>
  );
};
