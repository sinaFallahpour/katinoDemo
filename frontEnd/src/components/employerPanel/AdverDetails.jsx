import React, { useEffect, useState } from "react";
import axios from "axios";
import { toast } from "react-toastify";
import API_ADDRESS from "../../API_ADDRESS";
import { MiniSpinner } from "../spinner/MiniSpinner";
import {
  DetailsContainer,
  DetailsContent,
  DetailsContext,
  DetailsTitle,
} from "./AdverDetails.styles";
import { cooperationType } from "../../enums/cooperationType";
import { educationDegree } from "../../enums/educationDegree";
import { workExperience } from "../../enums/workExperience";
import { gender } from "../../enums/gender";
import { salary } from "../../enums/salary";
import { adverStatus } from "../../enums/adverStatus";
import ReactHtmlParser from "react-html-parser";

const AdverDetails = ({ adverId }) => {
  const [loading, setLoading] = useState(false);
  const [findCity, setFindCity] = useState(false);
  const [data, setData] = useState({});

  useEffect(() => {
    setLoading(true);
    adverId &&
      axios
        .get(
          API_ADDRESS + `Adver/AdverDetails`,
          { params: { id: parseInt(adverId) } },
          {
            headers: {
              Authorization: `bearer ${localStorage.getItem("JWT")}`,
            },
          }
        )
        .then(({ data }) => {
          const cityId = data.resul.city;
          setData(data.resul);

          axios.get(API_ADDRESS + "Account/GetCities").then((res) => {
            res.data.resul.map((item) => {
              parseInt(cityId) === item.cityDivisionCode
                ? setFindCity(`${item.provinceName} - ${item.cityName}`)
                : setFindCity(" - ");
            });
          });

          setLoading(false);
        })
        .catch((err) => {
          err.response.data.message &&
            err?.response?.data?.message.map((e) => {
              toast.error(e);
            });

          setLoading(false);
        });
  }, []);

  return (
    <>
      {loading ? (
        <MiniSpinner />
      ) : data ? (
        <DetailsContainer>
          <DetailsContent className="modalContaierOfAdver">
            <DetailsContext>
              <DetailsTitle>
                <i className="fas fa-bullseye c-success sml-1"></i>
                ?????????? ????????:{" "}
                {data.adverStatus ? adverStatus(data.adverStatus) : " - "}
                {" - "}
                {data.isImmediate ? data.isImmediate : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="c-success fas fa-building sml-1"></i>
                ?????? ????????: {data.companyName ? data.companyName : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="fas fa-file-alt c-success sml-1"></i>
                ?????????? ?????????? ???????????? ????????:{" "}
                {data.companyDescription
                  ? ReactHtmlParser(data.companyDescription)
                  : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="fas fa-clipboard c-success sml-1"></i>
                ?????????? ????????: {data.title ? data.title : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="fas fa-file-alt c-success sml-1"></i>
                ?????????????? ????????:{" "}
                {data.descriptionOfJob
                  ? ReactHtmlParser(data.descriptionOfJob)
                  : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="fa fa-map-marker c-success sml-1"></i>
                {data.city}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="fa fa-tasks c-success sml-1"></i>
                ???????? ????????????:{" "}
                {data.feildOfActivity ? data.feildOfActivity : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="c-success fas fa-money-bill-alt sml-1"></i>
                ?????????? ????????: {data.salary ? salary(data.salary) : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="c-success fas fa-handshake sml-1"></i>
                ?????? ??????????????:{" "}
                {data.typeOfCooperation
                  ? cooperationType(data.typeOfCooperation)
                  : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="c-success fas fa-people-arrows sml-1"></i>
                ??????????: {data.gender ? gender(data.gender) : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="c-success fa fa-graduation-cap sml-1"></i>
                ?????????? ??????????????:{" "}
                {data.degreeOfEducation
                  ? educationDegree(data.degreeOfEducation)
                  : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="c-success fas fa-address-card sml-1"></i>
                ?????????? ?????????? ??????:{" "}
                {data.workExperience
                  ? workExperience(data.workExperience)
                  : " - "}
              </DetailsTitle>
            </DetailsContext>

            <DetailsContext>
              <DetailsTitle>
                <i className="c-success fas fa-user-shield sml-1"></i>
                ?????????? ???????? ??????????: {data.military ? data.military : " - "}
              </DetailsTitle>
            </DetailsContext>

         
          </DetailsContent>
        </DetailsContainer>
      ) : (
        "?????????? ???????? ??????"
      )}
    </>
  );
};

export { AdverDetails };
