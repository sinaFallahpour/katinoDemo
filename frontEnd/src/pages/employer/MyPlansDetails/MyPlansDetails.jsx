import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import API_ADDRESS from "../../../API_ADDRESS";
import { toast } from "react-toastify";
import { MiniSpinner } from "../../../components/spinner/MiniSpinner";

const MyPlansDetails = () => {
  const [list, setList] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
    axios
      .get(
        API_ADDRESS + `Plan/GetPlanInfo`,
        {},
        {
          headers: {
            Authorization: `bearer ${localStorage.getItem("JWT")}`,
          },
        }
      )
      .then(({ data }) => {
        setList([data.resul]);

        setLoading(false);
      })
      .catch((err) => {
        err?.response?.data?.message.map((e) => {
          toast.error(e);
        });

        setLoading(false);
      });
  }, []);

  const FixedNumber = (num) => {
    return Math.floor(num);
  };

  return (
    <>
      {loading && <MiniSpinner />}

      <section className="dashboard container-fluid spx-2 smt-10 spx-lg-10">
        <div className="row">
          <div className="bg-white srounded-md sshadow w-100 sp-2">
            <nav className="navbar navbar-expand-lg pr-0 py-0">
              <div
                className="collapse navbar-collapse d-none d-lg-block"
                id="navbarSupportedContent"
              >
                <ul className="navbar-nav ml-auto">
                  <li className="nav-item smr-lg-4">
                    <Link
                      className="nav-link position-relative c-grey ir-r fs-m p-0 active"
                      style={{ color: "#00BCD4 !important" }}
                      to="/Employer/Dashboard/Plans"
                    >
                      تعرفه ها
                    </Link>
                  </li>

                  <li className="nav-item smr-lg-4">
                    <Link
                      className="nav-link position-relative c-grey ir-r fs-m p-0 active"
                      style={{ color: "#00BCD4 !important" }}
                      to="/Employer/History/Payment"
                    >
                      تاریخچه حساب
                    </Link>
                  </li>

                  <li className="nav-item smr-lg-4">
                    <Link
                      className="nav-link position-relative ir-r fs-m p-0 active"
                      to="/Employer/Dashboard/Plans"
                    >
                      اطلاعات اشتراک من
                    </Link>
                  </li>
                </ul>
              </div>
            </nav>
          </div>
        </div>

        <div
          style={{ marginTop: "30px" }}
          className="bg-white sbs-shadow srounded-md sp-2"
        >
          <header className="header d-lg-flex w-100 justify-content-lg-between align-items-lg-center">
            <h2 className="ir-b fs-s c-dark text-right smb-2 mb-lg-0">
              اطلاعات اشتراک من
            </h2>
          </header>

          <hr className="smy-2" />

          {list.length === 0 ? (
            <span className="ir-r fs-s c-regular text-center d-block">
              اشتراکی برای نمایش وجود ندارد.
            </span>
          ) : (
            <div className="table-responsive">
              <table className="table mb-0">
                <thead>
                  <tr>
                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      #
                    </th>
                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      نام طرح
                    </th>
                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      تعداد کل آگهی ها
                    </th>

                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      تعداد آگهی های باقی مانده
                    </th>
                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      تعداد کل آگهی های فوری
                    </th>
                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      تعداد آگهی های فوری باقی مانده
                    </th>
                    
                    
                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      تعداد کل آگهی های اسطوری ساز
                    </th>



                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      تا پایان دوره
                    </th>
                  </tr>
                </thead>
                <tbody>
                  {list.map((item, index) => (
                    <tr key={index}>
                      <td className="ir-r c-regular fs-s">{`#${index + 1}`}</td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.planName}
                        </span>
                      </td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.planAdverCount > 1000
                            ? "بی نهایت"
                            : `${item.planAdverCount} عدد`}
                        </span>
                      </td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.remainingAdversCount > 1000
                            ? "بی نهایت"
                            : `${item.remainingAdversCount} عدد`}
                        </span>
                      </td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.planImmediateAdverCount}
                        </span>
                      </td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.remainingImmediateAdversCount}
                        </span>
                      </td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.planStoryAdverCount}
                        </span>
                      </td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {FixedNumber(item.remainingDays)} روز
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </section>
    </>
  );
};

export { MyPlansDetails };
