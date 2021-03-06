import React, { useEffect, useState } from "react";
import axios from "axios";
import API_ADDRESS from "../../API_ADDRESS";
import Select from "react-select";
import CKEditor from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { Link, useHistory, useLocation, useParams } from "react-router-dom";
import { Formik, Field, Form, ErrorMessage, useField } from "formik";
import { CreateAdValidate } from "../../core/validation/createAd";
import {
  cooperationType,
  salaries,
  sex,
  exprience,
  education,
  initialData,
} from "./createAdData";
import { MiniSpinner } from "../spinner/MiniSpinner";
import "./Field.style.css";
import Swal from "sweetalert2";
import { toast } from "react-toastify";
import parse from 'react-html-parser';


export const EditAdverField = () => {
  const [initialState, setInitialState] = useState(initialData);
  const [categories, setCategories] = useState([]);
  const [cities, setCities] = useState([]);
  const [loading, setLoading] = useState(false);
  const [showMilitaryInfo, set_showMilitaryInfo] = useState(false);

  const history = useHistory();

  useEffect(() => {
    const params = new URLSearchParams(window.location.search);
    const adverId = params.get("AdverId");

    adverId &&
      axios
        .get(
          API_ADDRESS + `Adver/LoadAdver`,
          { params: { id: adverId } },
          {
            headers: {
              Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
            },
          }
        )
        .then(({ data }) => {

          set_showMilitaryInfo(data.resul.gender === 2 ? true : false);

          setInitialState({
            id: parseInt(adverId),
            fieldOfActivity: data.resul.fieldOfActivity || "",
            title: data.resul.title || "",
            city: data.resul.city || "",
            typeOfCooperation: data.resul.typeOfCooperation || 0,
            salary: data.resul.salary || 0,
            workExperience: data.resul.workExperience || 0,
            degreeOfEducation: data.resul.degreeOfEducation || 0,
            gender: data.resul.gender || 0,
            military: data.resul.military || "",
            descriptionOfJob: data.resul.descriptionOfJob || "",
            email: data.resul.email || "",
            phoneNumber: data.resul.phoneNumber || "",
            address: data.resul.address || "",
          });
        });

    const fetchData = async () => {
      const cities = [];

      let fieldOptions = [];
      const circle_symbol = parse('&#9679');
      const dash_symbol = parse('&#8211');
      const space_symbol = parse("&nbsp;");

      axios.get(API_ADDRESS + "Categories/GetAllCategories2").then((res) => {
        res.data.resul.forEach((item, index) => {
          if (item?.children.length > 0) {
            fieldOptions.push({ value: item.id, label: circle_symbol + space_symbol + item.name });
            item.children.forEach(item_child => {
              fieldOptions.push({ value: item_child.id, label: space_symbol + space_symbol + space_symbol + dash_symbol + space_symbol + item_child.name });
            })
          }
          else {
            fieldOptions.push({ value: item.id, label: circle_symbol + space_symbol + item.name });
          }
        });
        // console.log(fieldOptions)
        setCategories(fieldOptions);
      });

      await axios.get(API_ADDRESS + "Account/GetCities").then((res) => {
        res.data.resul.map((item) => {
          cities.push({
            // value: item.cityDivisionCode,
            value: `${item.provinceName}?? ${item.cityName}`,
            label: `${item.provinceName}?? ${item.cityName}`,
          });
        });
      });
      setCities(cities);
    };

    fetchData();
  }, []);

  const submitHandler = (values) => {
    setLoading(true);
    axios
      .post(API_ADDRESS + "Adver/EditAdver", values, {
        headers: {
          Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
        },
      })
      .then(() => {
        setLoading(false);
        Swal.fire({
          icon: "success",
          title: "???????????? ???????? ???? ???????????? ?????? ????",
          showConfirmButton: false,
          timer: 1750,
        });
      })
      .catch((err) => {
        err.response.data.message.map((er) => toast.error(er));
        setLoading(false);
      });
  };

  const MySelect = ({ label, options, ...props }) => {
    const [field, meta, helpers] = useField(props);
    var name = props.name

    return (
      <div>
        <Select
          {...field}
          {...props}
          options={options}
          value={
            options
              ? options.find((option) => option.value === field.value)
              : ""
          }
          onChange={(option) => {
            if (name === "gender") {
              if (option.value === 2 && option?.label === "??????") {
                set_showMilitaryInfo(true);
              } else set_showMilitaryInfo(false);
            }
            helpers.setValue(option.value)
          }}
        />
        <ErrorMessage
          component="div"
          className="errorMessage"
          name={props.name}
        />
      </div>
    );
  };

  const MyCheckbox = ({ ...props }) => {
    const [, meta] = useField({ ...props, type: "radio" });
    return (
      <>
        <div className="form-check form-check-inline">
          <label className="checkbox form-check-label">
            <Field
              className="form-check-input"
              type="radio"
              name="military"
              value={props.value}
            />
            {props.value}
          </label>
        </div>
      </>
    );
  };

  const MyTextAreaInput = ({ ...props }) => {
    const [, meta, helpers] = useField(props);
    return (
      <>
        <label className="checkbox form-check-label">{props.label}</label>
        <CKEditor
          className="cke_rtl"
          editor={ClassicEditor}
          data={initialState.descriptionOfJob}
          config={{
            toolbar: [
              "|",
              "bold",
              "italic",
              "numberedList",
              "bulletedList",
              "|",
              "undo",
              "redo",
            ],
            removePlugins: ["Heading", "Link"],
            language: "fa",
          }}
          onBlur={(_, editor) => {
            const data = editor.getData();
            helpers.setValue(data);
          }}
        />
        <ErrorMessage
          component="div"
          className="errorMessage"
          name={props.name}
        />
      </>
    );
  };

  return (
    <>
      {loading && <MiniSpinner />}
      <Formik
        initialValues={initialState}
        validationSchema={CreateAdValidate}
        onSubmit={(values) => {
          submitHandler(values);
        }}
        enableReinitialize={true}
      >
        <Form className="w-100">
          <div className="row">
            {/* title  */}
            <div className="col-12 smb-2">
              <label className="ir-r d-block text-right smb-1">
                ?????????? ????????
              </label>
              <div className="form-group mb-0">
                <Field
                  name="title"
                  className="form-control ir-r shadow-none"
                  placeholder="?????????? ????????"
                  type="text"
                />
                <ErrorMessage
                  component="div"
                  className="errorMessage"
                  name="title"
                />
              </div>
            </div>

            <div className="col-6 smb-2">
              <label className="ir-r d-block text-right smb-1">
                ??????????
              </label>
              <div className="form-group mb-0">
                <Field
                  name="email"
                  className="form-control ir-r shadow-none"
                  placeholder="??????????"
                  type="text"
                />
                <ErrorMessage
                  component="div"
                  className="errorMessage"
                  name="email"
                />
              </div>
            </div>

            <div className="col-6 smb-2">
              <label className="ir-r d-block text-right smb-1">
                ?????????? ????????
              </label>
              <div className="form-group mb-0">
                <Field
                  name="phoneNumber"
                  className="form-control ir-r shadow-none"
                  placeholder="?????????? ????????"
                  type="text"
                />
                <ErrorMessage
                  component="div"
                  className="errorMessage"
                  name="phoneNumber"
                />
              </div>
            </div>



            {/* fieldOfActivity  */}
            <div className="col-12 col-lg-6 smb-2 ir-r">
              <label className="ir-r d-block text-right smb-1">
                ???????? ???????? ????????
              </label>
              <MySelect
                name="fieldOfActivity"
                placeholder="???????? ???????? ???????????? ???? ?????????? ????????..."
                options={categories}
              />
            </div>

            {/* city  */}
            <div className="col-12 col-lg-6 smb-2 ir-r">
              <label className="ir-r d-block text-right smb-1">
                ???????????? ?????????? ?? ??????
              </label>
              <MySelect
                name="city"
                placeholder="?????? ???????????? ???? ?????????? ????????..."
                options={cities}
              />
            </div>

            <div className="col-12 smb-2 ir-r">
              <label className="ir-r d-block text-right smb-1">
                ????????
              </label>
              <Field
                name="address"
                className="form-control ir-r shadow-none"
                placeholder="????????..."
                type="text"
              />
              <ErrorMessage
                component="div"
                className="errorMessage"
                name="address"
              />
            </div>

            {/* typeOfCooperation  */}
            <div className="col-12 col-lg-6 smb-2 ir-r">
              <label className="ir-r d-block text-right smb-1">
                ?????? ????????????
              </label>
              <MySelect
                name="typeOfCooperation"
                placeholder="?????? ???????????? ???? ???????????? ????????..."
                options={cooperationType}
              />
            </div>

            {/* salary  */}
            <div className="col-12 col-lg-6 smb-2 ir-r">
              <label className="ir-r d-block text-right smb-1">
                ?????????? ????????
              </label>
              <MySelect
                name="salary"
                placeholder="?????????? ???????? ???? ???????????? ????????..."
                options={salaries}
              />
            </div>

            {/* descriptionOfJob  */}
            <div className="col-12 smb-2 ir-r">
              <MyTextAreaInput label="?????????????? ????????" name="descriptionOfJob" />
            </div>

            {/* gender  */}
            <div className="col-12 col-lg-6 smb-2 ir-r">
              <label className="ir-r d-block text-right smb-1">??????????</label>
              <MySelect
                name="gender"
                placeholder="?????????? ???? ???????????? ????????..."
                options={sex}
              />
            </div>

            {/* work exprience  */}
            <div className="col-12 col-lg-6 smb-2 ir-r">
              <label className="ir-r d-block text-right smb-1">
                ?????????? ?????? ??????????
              </label>
              <MySelect
                name="workExperience"
                placeholder="?????????? ?????? ???????? ???????? ???? ???????????? ????????..."
                options={exprience}
              />
            </div>

            {/* degreeOfEducation  */}
            <div className="col-12 col-lg-6 smb-2 ir-r">
              <label className="ir-r d-block text-right smb-1">
                ?????????? ???????? ????????????
              </label>
              <MySelect
                name="degreeOfEducation"
                placeholder="?????????? ???????? ???????????? ???????? ???????? ???? ???????????? ????????..."
                options={education}
              />
            </div>

            {/* military  */}
            {showMilitaryInfo && <div className="col-12 col-lg-6 smb-2 ir-r">
              <label className="ir-r d-block text-right smb-1">
                ?????????? ???????? ??????????
              </label>
              <MyCheckbox name="military" value="?????? ????????" />
              <MyCheckbox name="military" value="??????????" />
              <MyCheckbox name="military" value="????????" />
              <MyCheckbox name="military" value="?????????? ???????? ?????????? ????????" />
              <ErrorMessage
                component="div"
                className="errorMessage"
                name="military"
              />
            </div>}

            {/* submit button  */}
            <div className="smt-3 col-12">
              <button

                type="submit"
                className="btn btn-success ir-r d-block w-50"
                style={{ margin: "0 auto" }}
              >
                ?????????????????? ????????
              </button>
            </div>
          </div>
        </Form>
      </Formik>
    </>
  );
};
