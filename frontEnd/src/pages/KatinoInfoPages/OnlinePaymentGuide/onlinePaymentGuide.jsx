import React, { useState, useEffect } from 'react';
import parse from 'html-react-parser';
import axios from 'axios';
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


const OnlinePaymentGuide = (props) => {

    const classes = useStyles();
    const [htmlTags, set_htmlTags] = useState(null);
    const [open, setOpen] = React.useState(false);

    const handleClose = () => {
        setOpen(false);
    };

    const handleToggle = () => {
        setOpen(!open);
    };


    useEffect(() => {
        const url = "https://panel.katinojob.ir/api/Setting/GetOnlinePaymentGuid";
        const getData = async () => {
            try {
                handleToggle();
                const resp = await axios.get(url);
                console.log(resp);
                set_htmlTags(resp?.data?.resul);
                window.scrollTo(0, 0);
                handleClose();
            } catch (ex) {
                handleClose();
            }
        };
        getData();
    }, []);

    return (
        <>
            <div style={{ marginTop: "130px" }}></div>
            {htmlTags && <div style={{borderRadius:"1rem"}} className="m-0 p-3 bg-white col-lg-10 col-11 mx-auto">
                <h1 style={{fontSize:"1.3rem"}} className=" m-0 my-3 p-0 text-right">راهنمای پرداخت آنلاین</h1>
                {parse(htmlTags.toString())}
            </div>}
            <Backdrop className={classes.backdrop} open={open}>
                <CircularProgress color="inherit" />
            </Backdrop>
        </>
    );
}

export { OnlinePaymentGuide };