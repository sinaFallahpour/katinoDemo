import React, { Component, useEffect, useState } from "react";
import { Post } from "../components/blog";
import * as service from "../components/blog";
import ADDRESS from "../ADDRESS";
import { MiniSpinner } from "../components/spinner/MiniSpinner";

// MUI Components :
import SwipeableViews from 'react-swipeable-views';
import { makeStyles, useTheme } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Typography from '@material-ui/core/Typography';
import Box from '@material-ui/core/Box';
import { useHistory } from "react-router-dom";
import queryString from "query-string";
import './blog.css';


function TabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`full-width-tabpanel-${index}`}
      aria-labelledby={`full-width-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box p={3}>
          <Typography>{children}</Typography>
        </Box>
      )}
    </div>
  );
}


function a11yProps(index) {
  return {
    id: `full-width-tab-${index}`,
    'aria-controls': `full-width-tabpanel-${index}`,
  };
}

const useStyles = makeStyles((theme) => ({
  root: {
    backgroundColor: theme.palette.background.paper,
    width: 500,
  },
}));



export const Blog = (props) => {
  const history = useHistory();
  // posts APIs and methods
  const api_employerPosts = "https://panel.katinojob.ir/api/Blogs/GetAllBlogForIndex?type=0";
  const api_employeePosts = "https://panel.katinojob.ir/api/Blogs/GetAllBlogForIndex?type=1";
  const api_publicPosts = "https://panel.katinojob.ir/api/Blogs/GetAllBlogForIndex?type=2";
  const api_newsPosts = "https://panel.katinojob.ir/api/Blogs/GetAllBlogForIndex?type=3";


  const [employer_posts, set_employer_posts] = useState(null);
  const [employee_posts, set_employee_posts] = useState(null);
  const [public_posts, set_public_posts] = useState(null);
  const [news_posts, set_news_posts] = useState(null);


  const get_employeePosts = async () => {
    try {
      await fetch(api_employeePosts, { method: "GET" })
        .then(response => response.json())
        .then(data => set_employee_posts(data.resul))
        .catch((ex) => console.log(ex));
    }
    catch (ex) {
      console.log(ex);
    }
  };
  const get_employerPosts = async () => {
    try {
      await fetch(api_employerPosts, { method: "GET" })
        .then(response => response.json())
        .then(data => set_employer_posts(data.resul))
        .catch((ex) => console.log(ex));
    }
    catch (ex) {
      console.log(ex);
    }
  };
  const get_publicPosts = async () => {
    try {
      await fetch(api_publicPosts, { method: "GET" })
        .then(response => response.json())
        .then(data => set_public_posts(data.resul))
        .catch((ex) => console.log(ex));
    }
    catch (ex) {
      console.log(ex);
    }
  };
  const get_newsPosts = async () => {
    try {
      await fetch(api_newsPosts, { method: "GET" })
        .then(response => response.json())
        .then(data => set_news_posts(data.resul))
        .catch((ex) => console.log(ex));
    }
    catch (ex) {
      console.log(ex);
    }
  };

  useEffect(() => {
    getUserType();

    get_employerPosts();
    get_employeePosts();
    get_publicPosts();
    get_newsPosts();
  }, []);

  useEffect(() => {
    getUserType();
  }, [history.location.search]);

  const getUserType = () => {
    const searchEx = queryString.parse(history.location.search)

    if (searchEx.user === "employer") {
      setValue(0);
    }
    else if (searchEx.user === "employee") {
      setValue(1);
    }
    else if (searchEx.user === "katino") {
      setValue(2);
    }
    else if (searchEx.user === "news") {
      setValue(3);
    }

  };


  //loading state
  const [loading, set_loading] = useState(false);

  // tabs state handling
  const [value, setValue] = React.useState(0);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  const handleChangeIndex = (index) => {
    setValue(index);
  };

   const styles = {
    '.MuiAppBar-root': {
      zIndex: '1 !important',
    },
  };

  return (
    <>
      {loading && <MiniSpinner />}
      <section className="blog-page container-fluid spx-2 spx-lg-10 smt-10 spt-3">
        <div className="row">
          <AppBar styles={styles}
          style={{ xIndex: "2" }} position="relative" color="default">
          <Tabs
            value={value}
            onChange={handleChange}
            indicatorColor="primary"
            textColor="primary"
            variant="fullWidth"
            aria-label="full width tabs example"
          >
            <Tab className="ir-b" style={{ fontSize: "1.3rem", fontWeight: "600" }} label="کارفرما" {...a11yProps(0)} />
            <Tab className="ir-b" style={{ fontSize: "1.3rem", fontWeight: "600" }} label="کارجو" {...a11yProps(1)} />
            <Tab className="ir-b" style={{ fontSize: "1.3rem", fontWeight: "600" }} label="کاتینو" {...a11yProps(2)} />
            <Tab className="ir-b" style={{ fontSize: "1.3rem", fontWeight: "600" }} label="اخبار" {...a11yProps(3)} />
          </Tabs>
          </AppBar>
        <SwipeableViews
          axis={'x-reverse'}
          index={value}
          onChangeIndex={handleChangeIndex}
          className="w-100"
        >
          <TabPanel value={value} index={0} dir={'rtl'}>
            {employer_posts && employer_posts.map((item, index) => (
              <div
                key={index}
                className={
                  index === employer_posts.length ? "mb-0 " : "smb-2 "
                }
              >
                <Post
                  id={item.id}
                  pic={
                    item.uploadPic !== null
                      ? `${ADDRESS}img/blog/${item.uploadPic}`
                      : "/img/sample-logo.svg"
                  }
                  title={item.title}
                  desc={item.content}
                  date={item.updateDate}
                />
              </div>
            ))}
          </TabPanel>
          <TabPanel value={value} index={1} dir={'rtl'}>
            {employee_posts && employee_posts.map((item, index) => (
              <div
                key={index}
                className={
                  index === employee_posts.length ? "mb-0 " : "smb-2 "
                }
              >
                <Post
                  id={item.id}
                  pic={
                    item.uploadPic !== null
                      ? `${ADDRESS}img/blog/${item.uploadPic}`
                      : "/img/sample-logo.svg"
                  }
                  title={item.title}
                  desc={item.content}
                  date={item.updateDate}
                />
              </div>
            ))}
          </TabPanel>
          <TabPanel value={value} index={2} dir={'rtl'}>
            {public_posts && public_posts.map((item, index) => (
              <div
                key={index}
                className={
                  index === public_posts.length ? "mb-0 " : "smb-2 "
                }
              >
                <Post
                  id={item.id}
                  pic={
                    item.uploadPic !== null
                      ? `${ADDRESS}img/blog/${item.uploadPic}`
                      : "/img/sample-logo.svg"
                  }
                  title={item.title}
                  desc={item.content}
                  date={item.updateDate}
                />
              </div>
            ))}
          </TabPanel>
          <TabPanel value={value} index={3} dir={'rtl'}>
            {news_posts && news_posts.map((item, index) => (
              <div
                key={index}
                className={
                  index === news_posts.length ? "mb-0 " : "smb-2 "
                }
              >
                <Post
                  id={item.id}
                  pic={
                    item.uploadPic !== null
                      ? `${ADDRESS}img/blog/${item.uploadPic}`
                      : "/img/sample-logo.svg"
                  }
                  title={item.title}
                  desc={item.content}
                  date={item.updateDate}
                />
              </div>
            ))}
          </TabPanel>
        </SwipeableViews>

        </div>
    </section>
      {/* set_loading(true);
    service
      .getBlogs()
      .then((res) => {
        set_posts(res.data.resul);
        set_loading(false);
      }); */}
    </>
  );

}
