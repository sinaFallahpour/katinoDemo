import axios from "axios";
import { toast } from "react-toastify";
import { APIs } from "./CONSTANTS.js";

export const getUserJobPreferenceForCurrentUser = async () => {
    try {
        const resp = await axios.get(APIs.seeJopPreferences, {
            headers: {
                'Authorization': "Bearer" + localStorage.getItem("JWT")
            }
        });
        // console.log(resp.data);

        if (resp.status === 200)
            return resp.data;
        else
            return null;
    }
    catch (ex) {
        if (ex.response) {
            console.log(ex.response);
            return null;
        }
        else {
            console.log(ex);
            return null;
        }
    }
};


export const get_cities = async () => {
    const list = [];
    try {
        const resp = await axios.get(APIs.getAllCities);
        if (resp.data?.resul) {
            resp.data.resul.forEach(item => {
                list.push(`${item.provinceName} , ${item.cityName}`);
            });
            return list;
        }
        return null;
    }
    catch (ex) {
        return null;
    }
};



export const get_jobCategories = async () => {
    const list = [];
    try {
        const resp = await axios.get(APIs.getAllJobCategories);
        if (resp.data.resul) {
            // resp.data.resul.forEach(item => {
            //     list.push({ value: item.id, label: item.name });
            // });
            return resp.data.resul;
        }
        return null;
    }
    catch (ex) {
        return null;
    }
};



export const editUserJobPreference = async (data) => {

    // const formData = {
    //     "id": data.id,
    //     "City": data.city,
    //     "TypeOfCooperation": data.typeOfCooperation,
    //     "Senioritylevel": data.senioritylevel,
    //     "Salary": data.salary,
    //     "Promotion": data.promotion,
    //     "Insurance": data.insurance,
    //     "EducationCourses": data.educationCourses,
    //     "FlexibleWorkingTime": data.flexibleWorkingTime,
    //     "HasMeel": data.hasMeel,
    //     "TransportationService": data.transportationService,
    //     "CategoryIds": data.categoryIds,
    // };

    const formData = new FormData();

    formData.append("id", data.id);
    formData.append("City", data.city);
    formData.append("TypeOfCooperation", data.typeOfCooperation);
    formData.append("Senioritylevel", data.senioritylevel);
    formData.append("Salary", data.salary);
    formData.append("Promotion",data.promotion);
    formData.append("Insurance", data.insurance);
    formData.append("EducationCourses", data.educationCourses);
    formData.append("FlexibleWorkingTime", data.flexibleWorkingTime);
    formData.append("HasMeel", data.hasMeel);
    formData.append("TransportationService", data.transportationService);

    data.categoryIds.forEach(item => {
        formData.append("CategoryIds", item);
    });


    try {
        const resp = await axios.post(APIs.editJopPreferences, formData, {
            headers: {
                'content-type': "multipart/form-data",
                'Authorization': "Bearer " + localStorage.getItem("JWT")
            }
        });

        return resp.data;
    }
    catch (ex) {
        if (ex.response) {
            console.log(ex.response)
            return ex.response?.data?.message;
        }
        else
            return null;
    }
};

export const addUserJobPreference = async (data) => {
    try {
        const resp = await axios.post(APIs.addJopPreferences, data, {
            headers: {
                'content-type': "application/json",
                'Authorization': "Bearer " + localStorage.getItem("JWT")
            }
        });

        return resp.data;
    }
    catch (ex) {
        if (ex.response) {
            console.log(ex.response)
            return ex.response?.data?.message;
        }
        else
            return null;
    }
};