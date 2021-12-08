import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import clsx from 'clsx';
import { makeStyles } from '@material-ui/core/styles';
import SwipeableDrawer from '@material-ui/core/SwipeableDrawer';
import Button from '@material-ui/core/Button';
import List from '@material-ui/core/List';
import Divider from '@material-ui/core/Divider';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import IconButton from '@material-ui/core/IconButton';
import ListItemText from '@material-ui/core/ListItemText';
import MenuIcon from '@material-ui/icons/Menu';
import CancelPresentationRoundedIcon from '@material-ui/icons/CancelPresentationRounded';

import useWindowDimensions from '../../../../core/utils/getWindowDimensios';
import { NavLink } from 'react-bootstrap';


const useStyles = makeStyles({
    list: {
        width: '100vw',
        height: "100vh"
    }
});



const OffCanvasNavbar = ({ itemsList = null, menuIconColor = "#444" }) => {

    const { width } = useWindowDimensions();
    const classes = useStyles();
    const [open, set_open] = useState(false);

    const toggleDrawer = (open) => (event) => {
        if (event && event.type === 'keydown' && (event.key === 'Tab' || event.key === 'Shift')) {
            return;
        }
        set_open(open);
    };

    const list = () => (
        <div
            style={{ overflowY: "scroll" }}
            dir="rtl"
            className={classes.list}
            role="presentation"
        // onClick={toggleDrawer(false)}
        // onKeyDown={toggleDrawer(false)}
        >
            <List>

                <IconButton style={{outline:"none"}}  onClick={toggleDrawer(false)}>
                    <CancelPresentationRoundedIcon htmlColor="red" />
                </IconButton>
                <Divider />
                {itemsList?.map((item) =>
                    <>
                        <ListItem key={item.id} className="text-right">
                            {item.component
                                ? item.component()
                                : item.onClick
                                    ? <ListItemText style={{ cursor: "pointer" }} className="text-right" onClick={item.onClick} primary={item.label} />
                                    : <Link onClick={toggleDrawer(item.hasInner ? true : false)} to={item.link} className="text-decoration-none m-0">{item.label}</Link>
                            }
                        </ListItem>
                        <Divider />
                    </>
                )}
            </List>

        </div>
    );



    return (
        <>
            {width < 992 &&
                <>
                    <IconButton style={{outline:"none"}} className="mx-0" onClick={toggleDrawer(true)}>
                        <MenuIcon style={{fontSize:"1.7rem"}} htmlColor={menuIconColor} />
                    </IconButton >

                    <SwipeableDrawer
                        anchor={'right'}
                        open={open}
                        onClose={toggleDrawer(false)}
                        onOpen={toggleDrawer(true)}
                    >
                        {list()}
                    </SwipeableDrawer>
                </>
            }
        </>
    );
}

export default OffCanvasNavbar;