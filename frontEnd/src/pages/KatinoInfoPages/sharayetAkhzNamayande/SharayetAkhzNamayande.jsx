import React, { useEffect, useState } from "react";
import { AboutUsContainer, Title, Description } from "./SharayetAkhzNamayande.style";
import { GetSharayetAkhzNamayande } from "../../../core/api/sharayetAkhzNamayande";
import { toast } from "react-toastify";
import { MiniSpinner } from "../../../components/spinner/MiniSpinner";
import ReactHtmlParser from "react-html-parser";

const SharayetAkhzNamayande = () => {
  const [data, setData] = useState(false);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
    try {
      const fetchData = async () => {
        const data = await GetSharayetAkhzNamayande();
        setData(data.resul);
      };
      fetchData();
      setLoading(false);
    } catch (err) {
      err?.response?.data?.message.map((e) => {
        toast.error(e);
      });

      setLoading(false);
    }
  }, []);
  return (
    <>
      {loading && <MiniSpinner />}
      <AboutUsContainer>
        <Title>   شرایط اخذ نماینده</Title>
        <Description>{data && ReactHtmlParser(data)}</Description>
      </AboutUsContainer>
    </>
  );
};

export { SharayetAkhzNamayande };
