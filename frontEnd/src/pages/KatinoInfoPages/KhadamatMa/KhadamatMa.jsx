import React, { useEffect, useState } from "react";
import { AboutUsContainer, Title, Description } from "./KhadamatMa.style";
import { GetKhadamatMa } from "../../../core/api/khadamatMa";
import { toast } from "react-toastify";
import { MiniSpinner } from "../../../components/spinner/MiniSpinner";
import ReactHtmlParser from "react-html-parser";
import axios from "axios";
import queryString from 'query-string';
import parse from 'html-react-parser';

const KhadamatMa = () => {
  const [data, set_data] = useState(false);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
    const { id } = queryString.parse(window.location.search);
    console.log(id);
    const getData = async () => {
      const url = "https://panel.katinojob.ir/api/Services/Detail";
      try {
        const resp = await axios.get(url + "?id=" + id);
        console.log(resp);

        if (resp.data.resul) {
          set_data(resp.data.resul);
        }
      } catch (ex) {
        if (ex.response) {
          console.log(ex.response);
        }
      }
      setLoading(false);
    };
    getData();


  }, [window.location.search]);
  return (
    <>
      {loading && <MiniSpinner />}
      <div style={{marginTop:"110px"}} className=" mb-4 p-2 col-lg-10 col-11 mx-auto d-flex flex-column justify-content-start align-items-stretch">
        <h1 style={{fontSize:"1.3rem"}} className="my-2 p-0 ir-r align-self-start">خدمات ما</h1>
        <ul style={{borderRadius:"1rem"}} className="my-2 p-3 d-flex flex-column justify-content-start align-items-stretch bg-white">
              {data && 
              <li key={data.id} id={"katinoService_" + data.id} className="m-0 my-2 mt-3 p-0 d-flex flex-column justify-content-start align-items-start">
                <h2 style={{fontSize:"1.1rem"}} className="m-0 p-0 ir-r">
                    {data.title}
                </h2>
                <div  className="my-2 text-justify ir-r">
                  {parse(data.description.toString())}
                </div>
              </li>}
        </ul>
      </div>
    </>
  );
};

export { KhadamatMa };
