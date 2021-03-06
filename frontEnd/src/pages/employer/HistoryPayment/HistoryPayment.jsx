import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import API_ADDRESS from "../../../API_ADDRESS";
import { toast } from "react-toastify";
import { MiniSpinner } from "../../../components/spinner/MiniSpinner";
import { EmployerHistoryPaymentDetails } from "./HistoryPaymentDetails";

const EmployerHistoryPayment = () => {
  const [list, setList] = useState([]);
  const [loading, setLoading] = useState(false);
  const [idNumber, setIdNumber] = useState();
  const [modalStatus, setModalStatus] = useState(false);

  useEffect(() => {
    setLoading(true);
    axios
      .get(
        API_ADDRESS + `Account/GetLastOfOrder`,
        {},
        {
          headers: {
            Authorization: `bearer ${localStorage.getItem("JWT")}`,
          },
        }
      )
      .then(({ data }) => {
        setList(data.resul);

        setLoading(false);
      })
      .catch((err) => {
        err?.response?.data?.message.map((e) => {
          toast.error(e);
        });

        setLoading(false);
      });
  }, []);

  function goto(event) {
    setModalStatus(false);
  }
  document.body.addEventListener("click", goto);

  return (
    <>
      {loading && <MiniSpinner />}
      {modalStatus && <EmployerHistoryPaymentDetails idNumber={idNumber} />}

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
                      ?????????? ????
                    </Link>
                  </li>

                  <li className="nav-item smr-lg-4">
                    <Link
                      className="nav-link position-relative ir-r fs-m p-0 active"
                      to="/Employer/History/Payment"
                    >
                      ?????????????? ????????
                    </Link>
                  </li>

                  <li className="nav-item smr-lg-4">
                    <Link
                      className="nav-link position-relative c-grey ir-r fs-m p-0 active"
                      style={{ color: "#00BCD4 !important" }}
                      to="/Employer/MyPlansDetails"
                    >
                      ?????????????? ???????????? ????
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
              ?????????????? ????????
            </h2>
          </header>

          <hr className="smy-2" />

          {list.length === 0 ? (
            <span className="ir-r fs-s c-regular text-center d-block">
              ?????????? ???????? ?????????? ???????? ??????????.
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
                      ?????? ????????
                    </th>
                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      ?????????? ????????
                    </th>

                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      ???????? ???????????? (??????????)
                    </th>
                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      ???????? + ???????????? (??????????)
                    </th>
                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      ?????? ??????????
                    </th>
                    <th
                      className="ir-b c-regular fs-s border-top-0"
                      scope="col"
                    >
                      ????????????
                    </th>
                  </tr>
                </thead>
                <tbody>
                  {list.map((item, index) => (
                    <tr key={index}>
                      <td className="ir-r c-regular fs-s">{`#${item.orderId}`}</td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.planName}
                        </span>
                      </td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.date}
                        </span>
                      </td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.price} ??????????
                        </span>
                      </td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.priceWithTax} ??????????
                        </span>
                      </td>
                      <td>
                        <span className="ir-r c-regular fs-s w-100 text-truncate">
                          {item.orderType}
                        </span>
                      </td>

                      <td className="ir-r c-regular fs-s">
                        <button
                          className="ir-r c-regular fs-s btn btn-light shadow-none sml-1"
                          onClick={() => {
                            setModalStatus(true);
                            setIdNumber(item.orderId);
                          }}
                        >
                          ????????????
                        </button>
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

export { EmployerHistoryPayment };
