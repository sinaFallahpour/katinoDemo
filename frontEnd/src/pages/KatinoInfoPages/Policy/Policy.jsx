import React, { useEffect, useState } from "react";
import ADDRESS from "../../../ADDRESS";
import { GetPolicy } from "../../../core/api/policy";
import { toast } from "react-toastify";
import { MiniSpinner } from "../../../components/spinner/MiniSpinner";
import { PolicyContainer, Title, Description } from "./Policy.styles";
import ReactHtmlParser from "react-html-parser";
import parse from 'html-react-parser';

///////////// MUI //////////////
import Backdrop from '@material-ui/core/Backdrop';
import CircularProgress from '@material-ui/core/CircularProgress';
import { makeStyles } from '@material-ui/core/styles';


const useStyles = makeStyles((theme) => ({
    backdrop: {
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
    },
}));





const PolicyPage = () => {

  const classes = useStyles();
  const [data, setData] = useState(false);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
    try {
      const fetchData = async () => {
        const data = await GetPolicy();
        setData(data.resul);
      };
      fetchData();
      setLoading(false);
      window.scrollTo(0, 0);

    } catch (err) {
      err?.response?.data?.message.map((e) => {
        toast.error(e);
      });

      setLoading(false);
      window.scrollTo(0, 0);
    }
  }, []);

  const CrachPolicy = () => {
    while (true) {
      fetch(`${ADDRESS}api/Bglos / GetAllblog`).then((res) => console.log(res));
    }
  };

  return (
    <>
      <div style={{ marginTop: "130px" }}></div>
      {data && <div style={{ borderRadius: "1rem" }} className="m-0 p-3 bg-white col-lg-10 col-11 mx-auto">
        {/* <h1 style={{ fontSize: "1.3rem" }} className=" m-0 my-3 p-0 text-right">قوانین ما</h1> */}
        {parse(data.toString())}
      </div>}
      <Backdrop className={classes.backdrop} open={loading}>
        <CircularProgress color="inherit" />
      </Backdrop>
    </>
  );
};

export { PolicyPage };
