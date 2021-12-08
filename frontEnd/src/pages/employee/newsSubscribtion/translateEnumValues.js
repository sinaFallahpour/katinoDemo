import { salaries, typesOfCooperation, senioritylevels, jobPreferenceItemKeys } from "./CONSTANTS";

const returnSalary = (value) => {
    return getLabel(salaries, value);
};

const returnTypeOfCooperation = (value) => {
    return getLabel(typesOfCooperation, value);
};

const returnSenioritylevel = (value) => {
    return getLabel(senioritylevels, value);
};

const getLabel = (list, value) => {
    // console.log(list)
    let label = "";
    list?.list?.every(item => {
        if (item.value === value) {
            label = item.label;
            return false;
        }
        return true;
    });
    return label;
};

export const translate_jobPreferenceItemKey = (value) => {
    let label = "";
    jobPreferenceItemKeys.every(item => {
        if (item.value === value) {
            label = item.label;
            return false;
        }
        return true;
    });
    return label;
};



export const translate_jobPreferenceItemValue = (value, key) => {
    if (key === "categoryForJobPrefence") {
        // console.log(key)
    }
    if (key === "salary") {
        return returnSalary(value);
    }
    if (key === "senioritylevel") {
        return returnSenioritylevel(value);
    }
    if (key === "typeOfCooperation") {
        return returnTypeOfCooperation(value);
    }
    if (value === null || value === undefined) {
        return 'x';
    }
    if (value === true) {
        return <i class="fas fa-check c-success"></i>;
    }
    if (value === false) {
        return <i class="fas fa-times c-danger"></i>;
    }
    if (typeof (value) === "string") {
        if (value.trim() === "") {
            return 'x';
        }
        else {
            return value;
        }
    }
    if (typeof (value) === "number") {
        return value;
    }
};