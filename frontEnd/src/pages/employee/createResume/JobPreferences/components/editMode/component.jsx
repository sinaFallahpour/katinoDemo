import React, { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import { get_cities, get_jobCategories, editUserJobPreference } from '../../API_SERVICES';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import Checkbox from '@material-ui/core/Checkbox';
import { senioritylevels, typesOfCooperation, salaries } from "../../CONSTANTS";
import { translate_jobPreferenceItemValue, translate_jobPreferenceItemKey } from "../../translateEnumValues";
import { FormControlLabel } from '@material-ui/core';


const EditMode = ({ getResomePercent, jobPreferences, handleStartLoading, handleStopLoading, apiCall_editInfo, apiCall_addInfo, set_jobPreferences, apiCall_seeInfo }) => {


    const [cities, set_cities] = useState(null);
    const [jobCategories, set_jobCategories] = useState(null);

    const [id, set_id] = useState(null);
    const [salary, set_salary] = useState(null);
    const [senioritylevel, set_senioritylevel] = useState(null);
    const [typeOfCooperation, set_typeOfCooperation] = useState(null);
    const [city, set_city] = useState(null);
    const [educationCourses, set_educationCourses] = useState(null);
    const [categoryForJobPrefence, set_categoryForJobPrefence] = useState(null);

    const [promotion, set_promotion] = useState(false);
    const [insurance, set_insurance] = useState(false);
    const [flexibleWorkingTime, set_flexibleWorkingTime] = useState(false);
    const [hasMeel, set_hasMeel] = useState(false);
    const [transportationService, set_transportationService] = useState(false);

    useEffect(() => {
        if (cities && jobCategories && jobPreferences) {
            handleStopLoading();
        }
    }, [cities, jobCategories]);

    useEffect(() => {

        const getData = async () => {

            const cats = await get_jobCategories();
            if (cats) {
                let categoriesList = [];
                cats.forEach(item => {
                    categoriesList.push({ value: item.id, label: item.name });
                });
                set_jobCategories(categoriesList);
            }

            const citiesData = await get_cities();
            console.log(citiesData)
            if (citiesData) {
                set_cities(citiesData);
            }

            if (jobPreferences) {

                let jlist = [];
                jobPreferences?.categoryForJobPrefence?.map(item =>
                    jlist = [...jlist, { value: item.categoryId, label: item.categoryName }]
                );
                set_categoryForJobPrefence(jlist);

                set_id(jobPreferences.id);
                set_city(jobPreferences.city);
                set_typeOfCooperation({ value: jobPreferences.typeOfCooperation, label: translate_jobPreferenceItemValue(jobPreferences.typeOfCooperation, 'typeOfCooperation') });
                set_senioritylevel({ value: jobPreferences.senioritylevel, label: translate_jobPreferenceItemValue(jobPreferences.senioritylevel, 'senioritylevel') });
                set_salary({ value: jobPreferences.salary, label: translate_jobPreferenceItemValue(jobPreferences.salary, 'salary') });
                set_insurance(jobPreferences.insurance);
                set_promotion(jobPreferences.promotion);
                set_transportationService(jobPreferences.transportationService);
                set_hasMeel(jobPreferences.hasMeel);
                set_flexibleWorkingTime(jobPreferences.flexibleWorkingTime);
                set_educationCourses(jobPreferences.educationCourses);


            }
            handleStopLoading();
        };
        getData();
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();

        handleStartLoading(true);

        const categoryIds = [];
        categoryForJobPrefence.forEach(item => categoryIds.push(item.value));

        const data = {
            id: id,
            city: city,
            promotion: promotion,
            insurance: insurance,
            flexibleWorkingTime: flexibleWorkingTime,
            hasMeel: hasMeel,
            transportationService: transportationService,
            salary: salary.value,
            senioritylevel: senioritylevel.value,
            typeOfCooperation: typeOfCooperation.value,
            educationCourses: educationCourses,
            categoryIds: categoryIds,
        }

        let res = null;

        if (jobPreferences.id) {
            res = await apiCall_editInfo(data);
        }
        else {
            delete data.id;
            res = await apiCall_addInfo(data);
        }

        if (res) {
            if (Array.isArray(res))
                toast.error(res[0]);
            else {
                toast.success("اطلاعات با موفقیت ذخیره شد .");
                getResomePercent();
                console.log(res)
            }
        }


        handleStopLoading(false);
    };

    return (
        <form onSubmit={handleSubmit} className={"position-relative m-0 p-0 ir-r d-flex  flex-column justify-content-start align-items-stretch"}>

            <div className="m-0 my-2 p-0 d-flex flex-column justify-content-start align-items-stretch">
                <label classNam="m-0 p-0 text-right"> شهر </label>
                <div className="m-0 mt-2 p-0">
                    {city && cities && <SingleSelect engLabel="city" list={cities} selectedItem={city} set_selected={(value) => { set_city(value); }} />}
                </div>
            </div>
            <div className="m-0 my-2 p-0 d-flex flex-column justify-content-start align-items-stretch">
                <label classNam="m-0 p-0 text-right"> سابقه کار </label>
                <div className="m-0 mt-2 p-0">
                    {senioritylevel && <SingleSelect engLabel="senioritylevel" list={senioritylevels.list} selectedItem={senioritylevel} set_selected={(value) => { set_senioritylevel(value); }} />}
                </div>
            </div>
            <div className="m-0 my-2 p-0 d-flex flex-column justify-content-start align-items-stretch">
                <label classNam="m-0 p-0 text-right">  نوع همکاری </label>
                <div className="m-0 mt-2 p-0">
                    {typeOfCooperation && <SingleSelect engLabel="typeOfCooperation" list={typesOfCooperation.list} selectedItem={typeOfCooperation} set_selected={(value) => { set_typeOfCooperation(value); }} />}
                </div>
            </div>
            <div className="m-0 my-2 p-0 d-flex flex-column justify-content-start align-items-stretch">
                <label classNam="m-0 p-0 text-right">   میزان حقوق </label>
                <div className="m-0 mt-2 p-0">
                    {salary && <SingleSelect engLabel="salary" list={salaries.list} selectedItem={salary} set_selected={(value) => { set_salary(value); }} />}
                </div>
            </div>
            <div className="m-0 my-2 p-0 d-flex flex-column justify-content-start align-items-stretch">
                <label classNam="m-0 p-0 text-right">   دسته های شغلی </label>
                <div className="m-0 mt-2 p-0">
                    {jobCategories && categoryForJobPrefence && <MultipleSelect engLabel="categoryForJobPrefence" list={jobCategories} selectedItems={categoryForJobPrefence} set_selected={(value) => { set_categoryForJobPrefence(value); }} />}
                </div>
            </div>

            <div className="m-0 my-3 p-0 d-flex flex-row flex-wrap justify-content-start align-items-center">

                <div className="m-0 ml-3 p-0"> <FormControlLabel
                    className="ir-r mr-0"
                    style={{ fontFamily: "iransans-regular" }}
                    control={<Checkbox className="mr-0 pl-1" checked={promotion} color="primary" onChange={(e) => { set_promotion(e.target.checked) }} name="promotion" />}
                    label={translate_jobPreferenceItemKey("promotion")}
                /></div>
                <div className="m-0 ml-3 p-0">
                    <FormControlLabel
                        className="ir-r mr-0"
                        style={{ fontFamily: "iransans-regular !important" }}
                        control={<Checkbox className="mr-0 pl-1" checked={insurance} color="primary" onChange={(e) => { set_insurance(e.target.checked) }} name="insurance" />}
                        label={translate_jobPreferenceItemKey("insurance")}
                    />
                </div>
                <div className="m-0 ml-3 p-0">
                    <FormControlLabel
                        className="ir-r mr-0"
                        style={{ fontFamily: "iransans-regular" }}
                        control={<Checkbox className="mr-0 pl-1" checked={flexibleWorkingTime} color="primary" onChange={(e) => { set_flexibleWorkingTime(e.target.checked) }} name="promotion" />}
                        label={translate_jobPreferenceItemKey("flexibleWorkingTime")}
                    />
                </div>
                <div className="m-0 ml-3 p-0">
                    <FormControlLabel
                        className="ir-r mr-0"
                        style={{ fontFamily: "iransans-regular" }}
                        control={<Checkbox className="mr-0 pl-1" checked={hasMeel} color="primary" onChange={(e) => { set_hasMeel(e.target.checked) }} name="hasMeel" />}
                        label={translate_jobPreferenceItemKey("hasMeel")}
                    />
                </div>
                <div className="m-0 ml-3 p-0">
                    <FormControlLabel
                        className="ir-r mr-0"
                        style={{ fontFamily: "iransans-regular !important" }}
                        control={<Checkbox className="mr-0 pl-1" checked={transportationService} color="primary" onChange={(e) => { set_transportationService(e.target.checked) }} name="insurance" />}
                        label={translate_jobPreferenceItemKey("transportationService")}
                    />
                </div>
                <div className="m-0 ml-3 p-0">
                    <FormControlLabel
                        className="ir-r mr-0"
                        style={{ fontFamily: "iransans-regular !important" }}
                        control={<Checkbox className="mr-0 pl-1" checked={educationCourses} color="primary" onChange={(e) => { set_educationCourses(e.target.checked) }} name="educationCourses" />}
                        label={translate_jobPreferenceItemKey("educationCourses")}
                    />
                </div>

            </div>
            <div className="m-0 my-3 p-0 ">
                <button type="submit" className="btn btn-success">ذخیره</button>
            </div>

        </form>
    );
};

const SingleSelect = ({ engLabel, list, selectedItem, set_selected }) => {
    console.log(selectedItem)

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
                else{
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
            getOptionLabel={(option) => option.label ? option.label : option}
            renderOption={(option) =>
                <div className="m-0 p-0 ir-r">
                    {option.label ? option.label : option}
                </div>
            }
            onChange={(event, newValue) => {
                set_selected(newValue);
            }}
            defaultValue={selectedItems}
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

export default EditMode;