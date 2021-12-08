import React, { useState, useEffect } from "react";
import ReactHtmlParser from "react-html-parser";
import axios from "axios";

import ADDRESS from "../../../../ADDRESS";
import API_ADDRESS from "../../../../API_ADDRESS";
import {
  EmployerDetailsContainer,
  EmployerDetailsHolder,
  LeftSideContainer,
  RightSideContainer,
  ImageContainer,
  Image,
  CompanyName,
  FullName,
  ContextContainer,
  Title,
  Description,
  FieldOfActivity,
} from "./EmployerDetails.style";
import { Link } from "react-router-dom";
// import EmployersDefaultLogo from '../../EditProfileEmployer/EmployersDefaultLogo.jpg';

const EmployersDefaultLogo = `${ADDRESS}Img/CompanyLogo/defult-employer-logo.jpg`

const EmployerDetails = ({ companies }) => {
  const [findCity, setFindCity] = useState();

  useEffect(() => {
    axios.get(API_ADDRESS + "Account/GetCities").then(({ data }) => {
      data.resul.map((item) => {
        companies.City === item.cityDivisionCode &&
          setFindCity(`${item.provinceName} - ${item.cityName}`);
      });
    });
  }, [companies]);

  const getEmployeesCount = (value) => {
    const enum1 = [
      { value: 1, label: "بین 2 تا 10 نفر" },
      { value: 2, label: "بین 11 تا 50 نفر" },
      { value: 3, label: "بین 51 تا 200 نفر" },
      { value: 4, label: "بین 201 تا 500 نفر" },
      { value: 5, label: "بین 501 تا 1000 نفر" },
      { value: 6, label: "بیشتر از 1000 نفر" },
    ];

    let label = "";
    enum1.every(item => {
      if (item.value === value) {
        label = item.label;
        return false;
      }
      return true;
    });

    return label;
  };

  return (
    <EmployerDetailsContainer>
      <EmployerDetailsHolder>
        <LeftSideContainer>
          <ImageContainer>
            {companies.Image
              ? <Image src={`${ADDRESS}img/CompanyLogo/${companies.Image}`} /> :
              <Image src={EmployersDefaultLogo} />}

          </ImageContainer>
          <CompanyName>
            <FullName>{companies.PersianFullName}</FullName>
            <FullName>{companies.EngFullName}</FullName>
          </CompanyName>
        </LeftSideContainer>
        <RightSideContainer>
          <ContextContainer
            style={{ width: "100%", direction: "ltr", margin: "0 15px" }}
          >
            <Link to="/Employer/EditProfile" className="btn btn-info">
              ویرایش
            </Link>
          </ContextContainer>
          <ContextContainer>
            <Title> مدیر عامل : </Title>
            <Description> {companies.ManagmentFullName} </Description>
          </ContextContainer>
          <ContextContainer>
            <Title> شهر و استان : </Title>
            <Description> {findCity} </Description>
          </ContextContainer>
          <ContextContainer>
            <Title> ایمیل : </Title>
            <Description> {companies.Email} </Description>
          </ContextContainer>
          <ContextContainer>
            <Title> شماره تماس : </Title>
            <Description> {companies.EmergencPhone} </Description>
          </ContextContainer>
          <ContextContainer>
            <Title> وب سایت : </Title>
            <Description> {companies.url} </Description>
          </ContextContainer>
          <ContextContainer>
            <Title> تعداد پرسنل : </Title>
            <Description> {getEmployeesCount(companies.NumberOfStaff)} </Description>
          </ContextContainer>
          <ContextContainer
            style={{
              flexDirection: "column",
              alignItems: "flex-start",
            }}
          >
            <Title> حوزه فعالیت : </Title>
            <Description>
              {companies.FieldOfActivity &&
                companies.FieldOfActivity.map(({ label, value }) => (
                  <FieldOfActivity key={value}>{label}</FieldOfActivity>
                ))}
            </Description>
          </ContextContainer>
          <ContextContainer
            style={{
              flexDirection: "column",
              alignItems: "flex-start",
            }}
          >
            <Title> توضیحات : </Title>
            <Description>
              {ReactHtmlParser(companies.ShortDescription)}
            </Description>
          </ContextContainer>
        </RightSideContainer>
      </EmployerDetailsHolder>
    </EmployerDetailsContainer>
  );
};

export { EmployerDetails };
