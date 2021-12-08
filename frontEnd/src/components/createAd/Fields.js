import React, { useEffect, useState } from "react";
import axios from "axios";
import API_ADDRESS from "../../API_ADDRESS";
import Select from "react-select";
import CKEditor from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { Link, useHistory } from "react-router-dom";
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
import "./Field.style.css";
import Swal from "sweetalert2";
import { toast } from "react-toastify";
import parse from 'react-html-parser';

export const Fields = () => {
  const [hasPlan, setHasPlan] = useState(false);
  const [planId, setPlanId] = useState();
  const [secondButton, setSecondButton] = useState(false);
  const [planDetails, setPlanDetails] = useState();
  const [categories, setCategories] = useState([]);
  const [cities, setCities] = useState([]);
  const [showMilitaryInfo, set_showMilitaryInfo] = useState(false);
  const [textValue, setText] = useState(false);



  const history = useHistory();

  useEffect(() => {
    axios
      .post(
        API_ADDRESS + "Adver/GetUserPlanWhenCreateAdver",
        {},
        {
          headers: {
            Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
          },
        }
      )
      .then((res) => {
        if (res.data.resul.hasPlan === false) {
          setHasPlan(false);
          setPlanId(res.data.resul.allPlanFor.id);
          setPlanDetails(res.data.resul.allPlanFor);
        } else {
          setHasPlan(true);
        }
      });

    const fetchData = async () => {
      const categoriesies = [];
      const cities = [];

      const circle_symbol = parse('&#9679');
      const dash_symbol = parse('&#8211');
      const space_symbol = parse("&nbsp;");


      let categories2 = [];

      await axios
        .get(API_ADDRESS + "Categories/GetAllCategories2")
        .then((res) => {
          res.data.resul.forEach((item, index) => {
            if (item?.children.length > 0) {
              categories2.push({ value: item.id, label: circle_symbol + space_symbol + item.name });
              item.children.forEach(item_child => {
                categories2.push({ value: item_child.id, label: space_symbol + space_symbol + space_symbol + dash_symbol + space_symbol + item_child.name });
              })
            }
          });
        });
      setCategories(categories2);


      await axios.get(API_ADDRESS + "Account/GetCities").then((res) => {
        res.data.resul.forEach((item) => {
          cities.push({
            // value: item.cityName,
            value: `${item.provinceName}، ${item.cityName}`,
            label: `${item.provinceName}، ${item.cityName}`,
          });
        });
      });
      setCities(cities);
    };

    fetchData();
  }, []);

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
              if (option.value === 2 && option?.label === "مرد") {
                set_showMilitaryInfo(true);
              } else set_showMilitaryInfo(false);
            }
            helpers.setValue(option.value);
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
              // name="military"
              name={props.name}
              value={props.value}
            />
            {props.text}
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

          data={textValue}
          onBlur={(_, editor) => {
            const data = editor.getData();
            helpers.setValue(data);
            setText(data)
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

  const submitHandler = (values) => {



    hasPlan === true &&
      axios
        .post(API_ADDRESS + "Adver/CreateAdver", values, {
          headers: {
            Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
          },
        })
        .then((res) => {
          history.push("/Employer/Dashboard");
          Swal.fire({
            icon: "success",
            title: "ثبت آگهی با موفقیت ثبت شد",
            showConfirmButton: false,
            timer: 1750,
          });
        })
        .catch((err) => {
          err.response.data.message &&
            err.response.data.message.map((er) => toast.error(er));
        });
  };

  const submitAddToDraft = (values) => {

    console.log(values)
    console.log(values)
    axios
      .post(API_ADDRESS + "Adver/SaveAdverToDraft", values, {
        headers: {
          Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
        },
      })
      .then((res) => {
        history.push("/Employer/Dashboard");
        Swal.fire({
          icon: "success",
          title: "آگهی در پیش نویس با موفقیت ذخیره شد",
          showConfirmButton: false,
          timer: 1750,
        });
      })
      .catch((err) => {
        err.response.data.message &&
          err.response.data.message.map((er) => toast.error(er));
      });
  };

  return (
    <>
      <Formik
        initialValues={initialData}
        validationSchema={CreateAdValidate}
        onSubmit={(values) => {
          secondButton === true
            ? submitAddToDraft(values)
            : submitHandler(values);

          setSecondButton(false);
        }}
      >

        {({ errors, touched }) => (

          <Form className="w-100">

            {/* 
        <div>{errors.fieldOfActivity}</div> 
        <div>{errors.title}</div> 
        <div>{errors.city}</div> 
        <div>{errors.typeOfCooperation}</div> 
        <div>{errors.salary}</div> 
        <div>{errors.workExperience}</div> 
        <div>{errors.degreeOfEducation}</div> 
        <div>{errors.gender}</div> 
        <div>{errors.military}</div> 
        <div>{errors.descriptionOfJob}</div> 
        <div>{errors.staticNumber}</div> 
        <div>{errors.phoneNumber}</div> 
        <div>{errors.email}</div> 
        <div>{errors.workTime}</div> 
        <div>{errors.address}</div> 
 */}

            <div className="row">
              {/* title  */}
              <div className="col-12 smb-2">
                <label className="ir-r d-block text-right smb-1">
                  عنوان آگهی
                </label>
                <div className="form-group mb-0">
                  <Field
                    name="title"
                    className="form-control ir-r shadow-none"
                    placeholder="عنوان آگهی"
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
                  ایمیل
                </label>
                <div className="form-group mb-0">
                  <Field
                    name="email"
                    className="form-control ir-r shadow-none"
                    placeholder="ایمیل"
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
                  شماره تماس
                </label>
                <div className="form-group mb-0">
                  <Field
                    name="phoneNumber"
                    className="form-control ir-r shadow-none"
                    placeholder="شماره تماس"
                    type="text"
                  />
                  <ErrorMessage
                    component="div"
                    className="errorMessage"
                    name="phoneNumber"
                  />
                </div>
              </div>


              {/* <div className="col-6 smb-2">
    <label className="ir-r d-block text-right smb-1">
      شماره ثابت
    </label>
    <div className="form-group mb-0">



      <Field
        name="staticNumber"
        className="form-control ir-r shadow-none"
        placeholder="شماره ثابت"
        type="text"
      />
      <ErrorMessage
        component="div"
        className="errorMessage"
        name="staticNumber"
      />
    </div>
  </div>


  <div className="col-6 smb-2">
    <label className="ir-r d-block text-right smb-1">
      ساعت کاری
    </label>
    <div className="form-group mb-0">

      <Field
        name="workTime"
        className="form-control ir-r shadow-none"
        placeholder="ساعت کاری"
        type="text"
      />
      <ErrorMessage
        component="div"
        className="errorMessage"
        name="workTime"
      />
    </div>
  </div> */}

              {/* fieldOfActivity  */}
              <div className="col-12 col-lg-6 smb-2 ir-r">
                <label className="ir-r d-block text-right smb-1">
                  دسته بندی شغلی
                </label>
                <MySelect
                  name="fieldOfActivity"
                  placeholder="دسته بندی مربوطه را جستجو کنید..."
                  options={categories}
                />
              </div>


              {/* city  */}
              <div className="col-12 col-lg-6 smb-2 ir-r">
                <label className="ir-r d-block text-right smb-1">
                  انتخاب استان و شهر
                </label>
                <MySelect
                  name="city"
                  placeholder="شهر مربوطه را جستجو کنید..."
                  options={cities}
                />
              </div>

              {/* address  */}
              <div className="col-12 smb-2 ir-r">
                <label className="ir-r d-block text-right smb-1">
                  آدرس
                </label>
                <Field
                  name="address"
                  className="form-control ir-r shadow-none"
                  placeholder="آدرس..."
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
                  نوع همکاری
                </label>
                <MySelect
                  name="typeOfCooperation"
                  placeholder="نوع همکاری را انتخاب کنید..."
                  options={cooperationType}
                />
              </div>

              {/* salary  */}
              <div className="col-12 col-lg-6 smb-2 ir-r">
                <label className="ir-r d-block text-right smb-1">
                  میزان حقوق
                </label>
                <MySelect
                  name="salary"
                  placeholder="میزان حقوق را انتخاب کنید..."
                  options={salaries}
                />
              </div>

              {/* descriptionOfJob  */}
              <div className="col-12 smb-2 ir-r">
                <MyTextAreaInput label="توضیحات آگهی" name="descriptionOfJob" />
              </div>

              {/* gender  */}
              <div className="col-12 col-lg-6 smb-2 ir-r">
                <label className="ir-r d-block text-right smb-1">جنسیت</label>
                <MySelect
                  name="gender"
                  placeholder="جنسیت را انتخاب کنید..."
                  options={sex}
                />
              </div>

              {/* work exprience  */}
              <div className="col-12 col-lg-6 smb-2 ir-r">
                <label className="ir-r d-block text-right smb-1">
                  سابقه کار مرتبط
                </label>
                <MySelect
                  name="workExperience"
                  placeholder="سابقه کار مورد نیاز را انتخاب کنید..."
                  options={exprience}
                />
              </div>

              {/* degreeOfEducation  */}
              <div className="col-12 col-lg-6 smb-2 ir-r">
                <label className="ir-r d-block text-right smb-1">
                  حداقل مدرک تحصیلی
                </label>
                <MySelect
                  name="degreeOfEducation"
                  placeholder="حداقل مدرک تحصیلی مورد نیاز را انتخاب کنید..."
                  options={education}
                />
              </div>

              {/* military  */}
              {showMilitaryInfo &&
                <div className="col-12 col-lg-6 smb-2 ir-r">
                  <label className="ir-r d-block text-right smb-1">
                    وضعیت نظام وظیفه
                  </label>
                  <MyCheckbox name="military" value="مهم نیست" text="مهم نیست" />
                  <MyCheckbox name="military" value="مشمول" text="مشمول" />
                  <MyCheckbox name="military" value="معاف" text="معاف" />
                  <MyCheckbox name="military" value="دارای کارت پایان خدمت" text="دارای کارت پایان خدمت" />
                  <ErrorMessage
                    component="div"
                    className="errorMessage"
                    name="military"
                  />
                </div>
              }


              {/* <div className="col-12 col-lg-6 smb-2 ir-r">
    <label className="ir-r d-block text-right smb-1">
      نوع
    </label>
    <MyCheckbox name="storySaz" value={"1"} text="استوری ساز" />
    <MyCheckbox name="storySaz" value={"0"} text="عادی" />
    <ErrorMessage
      component="div"
      className="errorMessage"
      name="storySaz"
    />
  </div> */}
              {/* submit button  */}
              <div className="smt-3 col-12">
                <div className="row d-lg-flex align-items-lg-center">
                  {hasPlan === false ? (
                    <div className="col-12 col-lg-6 smb-2 mb-lg-0 ir-r">

                    </div>
                  ) : (
                    ""
                  )}

                  <div className="col-12 col-lg-3 mt-0 smt-lg-3 smb-2 mb-lg-0 ir-r mr-auto">
                    <button
                      type="submit"
                      className="btn btn-light ir-r d-block w-100"
                      onClick={() => setSecondButton(true)}
                    >
                      ذخیره در پیش نویس
                    </button>
                  </div>

                  <div className="col-12 col-lg-3 mt-0 smt-lg-3 smb-2 mb-lg-0 ir-r ml-auto">
                    {hasPlan === false ? (
                      // <button type="button"
                      //   className="btn btn-warning ir-r d-block w-100"
                      //   // to={`/Employer/Dashboard/Plans/${planId}/Payment`}
                      //   // to={`/Employer/Dashboard/Plans`}
                      //   onClick={submitHandler}
                      // >
                      //   پرداخت هزینه
                      // </button>
                      <Link
                        className="btn btn-warning ir-r d-block w-100"
                        // to={`/Employer/Dashboard/Plans/${planId}/Payment`}
                        to={`/Employer/Dashboard/Plans`}
                      >
                        پرداخت هزینه
                      </Link>
                    ) : (
                      <button
                        type="submit"
                        className="btn btn-success ir-r d-block w-100"
                      >
                        فعالسازی و انتشار
                      </button>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </Form>
        )}

      </Formik>
    </>
  );
};
