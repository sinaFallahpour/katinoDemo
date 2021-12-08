import axios from "axios";

import API_ADDRESS from "../../API_ADDRESS";

const EndPoint = API_ADDRESS + "Setting/GetSharayetAkhzNamayande";

export const GetSharayetAkhzNamayande = async () => {
    const { data } = await axios.get(EndPoint);

    return data;
};
