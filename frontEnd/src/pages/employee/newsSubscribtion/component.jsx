import React, { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import parse from 'react-html-parser';
import { get_cities, get_jobCategories, subscribeIn_getNewAdvers, getPersonalInfo } from './API_SERVICES';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { senioritylevels, typesOfCooperation, salaries } from "./CONSTANTS";
import { translate_jobPreferenceItemValue, translate_jobPreferenceItemKey } from "./translateEnumValues";
import Backdrop from '@material-ui/core/Backdrop';
import CircularProgress from '@material-ui/core/CircularProgress';
import Button from '@material-ui/core/Button';
import Select from "react-select";
import './style/style.css';

import { makeStyles } from '@material-ui/core/styles';
import { useHistory } from 'react-router-dom';

const useStyles = makeStyles((theme) => ({
    backdrop: {
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
    },
}));

const NewsSubscribtion = (props) => {

    const history = useHistory();

    const flex_column_start_stretch = " d-flex flex-column justify-content-start align-items-stretch";
    const flex_row_start_stretch = " d-flex flex-row flex-wrap justify-content-start align-items-stretch";
    const flex_row_center_center = " d-flex flex-row justify-content-center align-items-center";

    const [cities, set_cities] = useState(null);
    const [jobCategories, set_jobCategories] = useState(null);

    const [selected_email, set_selected_email] = useState(null);
    const [selected_jobTitle, set_selected_jobTitle] = useState(null);
    const [selected_sendPeriod, set_selected_sendPeriod] = useState("روزانه");
    const [selected_typeOfCooperation, set_selected_typeOfCooperation] = useState(null);
    const [selected_categories, set_selected_categories] = useState(null);
    const [selected_cities, set_selected_cities] = useState(null);


    const classes = useStyles();
    const [open, setOpen] = useState(false);

    const handleClose_loading = () => {
        setOpen(false);
    };

    const handleOpen_loading = () => {
        setOpen(!open);
    };

    useEffect(() => {

        const { feildOfActivity, typeOfCooperation, jobTitle, city } = history.location.state;

        set_selected_jobTitle(jobTitle);

        typesOfCooperation.list?.every(item => {
            if (item.value.toString() === typeOfCooperation.toString()) {
                set_selected_typeOfCooperation({ ...item });
            }
            return true;
        });

        const load_categories = async () => {
            const feildOfActivity = '1257';

            const circle_symbol = parse('&#9679');
            const dash_symbol = parse('&#8211');
            const space_symbol = parse("&nbsp;");

            const cats = await get_jobCategories();

            if (cats) {
                let categoriesList = [];

                await cats.forEach((item, index) => {
                    if (item?.children.length > 0) {
                        categoriesList.push({ value: item.id, label: circle_symbol + space_symbol + item.name });
                        if (item.id.toString() === feildOfActivity) {
                            console.log(item.name)
                            set_selected_categories([{ value: item.id, label: circle_symbol + space_symbol + item.name }]);
                        }
                        item.children.forEach(item_child => {
                            categoriesList.push({ value: item_child.id, label: space_symbol + space_symbol + space_symbol + dash_symbol + space_symbol + item_child.name });
                            if (item_child.id.toString() === feildOfActivity) {
                                set_selected_categories([{ value: item_child.id, label: space_symbol + space_symbol + space_symbol + dash_symbol + space_symbol + item_child.name }]);
                            }
                        });
                    }
                    else{
                        categoriesList.push({ value: item.id, label: circle_symbol + space_symbol + item.name });
                        if (item.id.toString() === feildOfActivity) {
                            console.log(item.name)
                            set_selected_categories([{ value: item.id, label: circle_symbol + space_symbol + item.name }]);
                        }
                    }
                });

                set_jobCategories(categoriesList);
            }

        };

        const load_cities = async () => {
            const citiesData = await get_cities();

            if (citiesData) {
                set_cities(citiesData);
            }
        };

        const load_personalInfo = async () => {
            const infoResp = await getPersonalInfo();
            if (infoResp?.data?.resul?.email) {
                set_selected_email(infoResp.data?.resul?.email);
                document.getElementById("emailForNews").setAttribute("disabled", true);
            }
        };

        const getData = async () => {

            handleOpen_loading();

            await load_categories();
            await load_cities();
            await load_personalInfo();


            handleClose_loading();
        };

        getData();

    }, []);


    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!selected_email) {
            toast.error("لطفا ایمیل خود را وارد کنید .");
            return;
        }
        if (!selected_categories) {
            toast.error("لطفا حداقل یک دسته بندی شغلی را انتخاب کنید .");
            return;
        }

        handleOpen_loading();

        let categoryIds = "";
        selected_categories?.forEach((item, index) => {
            if (index < selected_categories.length - 1)
                categoryIds += ((item.value) + "_");
            else
                categoryIds += item.value;
        });

        let selectedCities = "";
        selected_cities?.forEach((item, index) => {
            if (index < selected_cities.length - 1)
                selectedCities += ((item.value) + ",");
            else
                selectedCities += item.value;
        });


        const data = {
            CategoryIds: categoryIds ? categoryIds : "",
            KeyWord: selected_jobTitle ? selected_jobTitle : "",
            TypeOfCooperation: selected_typeOfCooperation?.value ? selected_typeOfCooperation?.value : "",
            Cities: selectedCities ? selectedCities : "",
            Email: selected_email ? selected_email : "",
        }

        const res = await subscribeIn_getNewAdvers(data);
        console.log(res)
        if (res) {
            toast.success("عضویت در خبرنامه با موفقیت انجام شد .");
        }
        else
            toast.error("عضویت در خبرنامه با خطا مواجه شد . لطفا دوباره تلاش کنید .");

        handleClose_loading();
    };

    return (
        <section className={"newAdversSubscribtion-area m-0 p-md-0 p-3 " + flex_row_center_center}>
            <div className={"newAdversSubscribtion-area__inner m-0 p-0 col-lg-8 col-md-10 col-12 mx-auto   " + flex_column_start_stretch}>
                <h1 className="newAdversSubscribtion-area__header m-0 my-2 p-0 text-center">فرصت‌های شغلی جدید در ایمیل شما!</h1>
                <p className="newAdversSubscribtion-area__desc m-0 my-2 p-0 text-center">اولین نفری که برای کارفرمایان رزومه ارسال می‌کند شما باشید. هر هفته فرصت‌های شغلی جدید برای شما ارسال می‌شود.</p>
                <form onSubmit={handleSubmit} className={"newAdversSubscribtion-area__fields m-0 my-2 p-2" + flex_row_start_stretch}>

                    <MembershipField required={true} label="دسته های شغلی" colSize="col-sm-6 col-12">
                        {jobCategories && <MultipleSelect engLabel="categoryForJobPrefence" list={jobCategories} selectedItems={selected_categories ? [...selected_categories] : []} set_selected={(value) => { set_selected_categories(value); }} />}
                    </MembershipField>

                    <MembershipField label="عنوان شغلی " colSize="col-sm-6 col-12">
                        <input value={selected_jobTitle} onChange={(e) => { set_selected_jobTitle(e.currentTarget.value) }} style={{ height: "52px", border: "1px solid #ccc", borderRadius: "5px", outline: "none" }} type="text" className="m-0 p-2 px-3 w-100" />
                    </MembershipField>

                    <MembershipField label="نوع قرارداد " colSize="col-sm-6 col-12">
                        {typesOfCooperation && selected_typeOfCooperation && <SingleSelect engLabel="typeOfCooperation" list={typesOfCooperation.list} selectedItem={selected_typeOfCooperation} set_selected={(value) => { set_selected_typeOfCooperation(value); }} />}
                    </MembershipField>

                    <MembershipField label="شهر" colSize="col-sm-6 col-12">
                        {cities && <MultipleSelect engLabel="city" list={cities} selectedItem={selected_cities ? [...selected_cities] : []} set_selected={(value) => { set_selected_cities(value); }} />}
                    </MembershipField>

                    <MembershipField label="ارسال به صورت " colSize="col-sm-6 col-12" fieldParentClasses={flex_row_start_stretch}>
                        <input id="daily" name="sendPeriod" type="radio" className="m-0 p-0 d-none" />
                        <label onClick={() => { set_selected_sendPeriod("روزانه") }} htmlFor="daily" className={"m-0 col-6" + flex_row_center_center} style={{ cursor: "pointer", border: "2px solid rgb(39, 60, 133)", height: "52px", backgroundColor: selected_sendPeriod === "روزانه" ? "rgb(39, 60, 133)" : "#fff", color: selected_sendPeriod === "روزانه" ? "#fff" : "rgb(39, 60, 133)" }}>روزانه</label>
                        <input id="weekly" name="sendPeriod" type="radio" className="m-0 p-0 d-none" />
                        <label onClick={() => { set_selected_sendPeriod("هفتگی") }} htmlFor="weekly" className={"m-0 col-6" + flex_row_center_center} style={{ cursor: "pointer", border: "2px solid rgb(39, 60, 133)", height: "52px", backgroundColor: selected_sendPeriod === "هفتگی" ? "rgb(39, 60, 133)" : "#fff", color: selected_sendPeriod === "هفتگی" ? "#fff" : "rgb(39, 60, 133)" }}>هفتگی</label>
                    </MembershipField>

                    <MembershipField required={true} label="ایمیل" colSize="col-sm-6 col-12">
                        <input id="emailForNews" value={selected_email} onChange={(e) => { set_selected_email(e.currentTarget.value) }} style={{ height: "52px", border: "1px solid #ccc", borderRadius: "5px", outline: "none" }} type="email" className="m-0 px-3 py-2 text-right w-100" />
                    </MembershipField>

                    <Button type="submit" className="m-0 my-3 mt-4 mx-auto py-2 px-3 btn btn-primary" variant="contained" color="primary">
                        دریافت فرصت های شغلی
                    </Button>

                </form>
            </div>
            <Backdrop className={classes.backdrop} open={open} >
                <CircularProgress color="inherit" />
            </Backdrop>
        </section>
    );
};


const MembershipField = (props) => {

    const flex_column_start_stretch = " d-flex flex-column justify-content-start align-items-stretch";
    const flex_row_start_center = " d-flex flex-row justify-content-start align-items-center";
    const { colSize, label, children, fieldParentClasses = "", required = false } = props;
    return (
        <div className={"m-0 my-2 p-2 " + colSize + " " + flex_column_start_stretch}>
            <label classNam={"m-0 p-0 text-right " + flex_row_start_center}>
                {label}
                {required
                    ? <span className="m-0 mr-1 p-0 text-danger">*</span>
                    : ''
                }
            </label>
            <div className={"m-0 mt-2 p-0 " + fieldParentClasses}>
                {children}
            </div>
        </div>
    );
};

const SingleSelect = ({ engLabel, list, selectedItem, set_selected }) => {

    return (
        <Autocomplete
            id={engLabel + "-select"}
            error={true}
            helperText="Example error"
            className="mb-2 pr-0 ir-r"
            style={{ width: "100%", height: "40px" }}
            options={list.map((item) => {
                if (item?.value) {
                    return {
                        value: item.value,
                        label: item.label,
                    };
                }
                else {
                    // console.log(item)
                    return (item);
                }
            })}
            getOptionLabel={(option) => option.label ? option.label : option}
            renderOption={(option) =>
                <div className="m-0 p-0 ir-r">
                    {option.label ? option.label : option}
                </div>
            }
            onChange={(event, newInputValue) => {
                if (newInputValue === null) {
                    set_selected(" ");
                }
                else {
                    set_selected(newInputValue);
                }
            }}

            defaultValue={selectedItem}
            renderInput={(params) => (
                <TextField
                    className="ir-r"

                    {...params}
                    // label="Choose a country"
                    variant="outlined"

                />
            )}
        />);
};

const MultipleSelect = ({ engLabel, list, selectedItems, set_selected }) => {

    return (
        <Autocomplete
            multiple
            id={engLabel + "-select"}
            className="mb-2 pr-0 ir-r"
            style={{ width: "100%" }}
            options={list.map((item) => {
                if (item.value) {

                    return {
                        value: item.value,
                        label: item.label,
                    };
                }
                else {
                    return (item);
                }
            })}
            // value={selectedItems ? selectedItems : []}
            defaultValue={selectedItems ? [...selectedItems] : []}
            getOptionLabel={(option) => option.label ? option.label : option}
            renderOption={(option) =>
                <div className="m-0 p-0 ir-r">
                    {option.label ? option.label : option}
                </div>
            }
            onChange={(event, newValue) => {
                // console.log(newValue);

                // // if (newValue[0].value ? newValue[0].value : newValue[0] === newValue[newValue.length - 1]) {
                // //     newValue.shift();
                // // }
                // // else {
                //     for (let i = 0; i < newValue?.length - 2; i++) {
                //         if (newValue[i].value ? newValue[i].value : newValue[i] === newValue[newValue.length - 1]) {
                //             newValue.pop();
                //             // newValue.removeAt(newValue.length - 1);
                //             break;
                //         }
                //     }
                // // }
                // set_selected(newValue);


                let repeated = false;
                if (selectedItems) {
                    selectedItems?.every(item => {

                        if (item?.value.toString() === newValue?.[0]?.value.toString()) {
                            repeated = true;
                            return false;
                        }

                        return true;
                    });
                    const s0 = selectedItems[0]?.value ? selectedItems[0]?.value : selectedItems[0];
                    const nL = newValue[newValue.length - 1].value ? newValue[newValue.length - 1].value : newValue[newValue.length - 1];
                    if (newValue.length > 1 && s0 === nL) {
                        repeated = true;
                        newValue.pop();
                    }
                }

                if (!repeated) {
                    set_selected(newValue);
                }
            }}
            renderInput={(params) => (
                <TextField
                    className="ir-r"

                    {...params}
                    // label="Choose a country"
                    variant="outlined"

                />
            )}
        />);
};

export { NewsSubscribtion };