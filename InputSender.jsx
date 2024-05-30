import React, {useEffect, useRef, useState} from 'react';
import {Animated, View, StyleSheet, PanResponder, Text, TouchableOpacity} from 'react-native';
import {Accelerometer} from 'expo-sensors';
import dgram from 'react-native-udp';

const InputSender = ({navigation,route}) => {

useEffect(()=>{
  return(()=>{
    if(subscription.obj){
      subscription.obj.remove();
    }
  });
},[]);


  const sliderPosition=useRef({x:0,y:0}).current;
  const socket=useRef(dgram.createSocket('udp4')).current;
  const subscription=useRef({obj:null}).current;

  const [isVisible, setIsVisible] = useState(true);

  const init = ()=>{
    socket.bind(11000);
    Accelerometer.setUpdateInterval(30);
    subscription.obj=Accelerometer.addListener(setData);

  };

  const setData=(posData)=>{
    
    //tick rate of data send is decided by the accelerometer
    
    const temp = posData.y;
    //y=16683.67x + 16350
    //VJOY_DATA_WHL
    const VJOY_DATA_WHL=Math.abs(Math.round((16683.67*posData.y)+16350));
    let VJOY_DATA_ACL=0;
    let VJOY_DATA_BRK=0;
    let VJOY_DATA_GAX=0;


    if(sliderPosition.x>=0){
      VJOY_DATA_ACL=sliderPosition.x;
      VJOY_DATA_BRK=0;
    }else{
      VJOY_DATA_ACL=0;
      VJOY_DATA_BRK=sliderPosition.x*-1;
    }

      VJOY_DATA_GAX=Math.round((0.5*sliderPosition.y)+16350);
   
    socket.send("VJOY_DATA,"+VJOY_DATA_WHL+","+VJOY_DATA_ACL+","+VJOY_DATA_BRK+","+VJOY_DATA_GAX,undefined,undefined,11000,route.params.ip);

  }

  




  
  const panResponder = React.useRef(
    PanResponder.create({
      // Ask to be the responder:
      onStartShouldSetPanResponder: (evt, gestureState) => true,
      onStartShouldSetPanResponderCapture: (evt, gestureState) =>
        true,
      onMoveShouldSetPanResponder: (evt, gestureState) => true,
      onMoveShouldSetPanResponderCapture: (evt, gestureState) =>
        true,

      onPanResponderGrant: (evt, gestureState) => {
        // The gesture has started. Show visual feedback so the user knows
        // what is happening!
        // gestureState.d{x,y} will be set to zero now
      },
      onPanResponderMove: (evt, gestureState) => {
        //sliderPosition.x=gestureState.dx;
        let temp = Math.round((235.25*gestureState.moveX)-45638.85);
        if(Math.abs(temp)>32700){
          temp=32700*Math.sign(temp);
        }
        sliderPosition.x=temp;
        //32700
        //ta
      },
      onPanResponderTerminationRequest: (evt, gestureState) =>
        true,
      onPanResponderRelease: (evt, gestureState) => {
        // The user has released all touches while this view is the
        // responder. This typically means a gesture has succeeded
        sliderPosition.x=0;
       
      },
      onPanResponderTerminate: (evt, gestureState) => {
        // Another component has become the responder, so this gesture
        // should be cancelled
      },
      onShouldBlockNativeResponder: (evt, gestureState) => {
        // Returns whether this component should block native components from becoming the JS
        // responder. Returns true by default. Is currently only supported on android.
        return true;
      },
    }),
  ).current;


  const otherPanResponder = React.useRef(
    PanResponder.create({
      // Ask to be the responder:
      onStartShouldSetPanResponder: (evt, gestureState) => true,
      onStartShouldSetPanResponderCapture: (evt, gestureState) =>
        true,
      onMoveShouldSetPanResponder: (evt, gestureState) => true,
      onMoveShouldSetPanResponderCapture: (evt, gestureState) =>
        true,

      onPanResponderGrant: (evt, gestureState) => {
        // The gesture has started. Show visual feedback so the user knows
        // what is happening!
        // gestureState.d{x,y} will be set to zero now
      },
      onPanResponderMove: (evt, gestureState) => {
        //sliderPosition.x=gestureState.dx;
        let temp = Math.round((235.25*gestureState.moveX)-45638.85);

      
        if(Math.abs(temp)>32700){
          temp=32700*Math.sign(temp);
        }
        sliderPosition.y=temp;
        //32700
        //ta
      },
      onPanResponderTerminationRequest: (evt, gestureState) =>
        true,
      onPanResponderRelease: (evt, gestureState) => {
        // The user has released all touches while this view is the
        // responder. This typically means a gesture has succeeded
        sliderPosition.y=0;
       
      },
      onPanResponderTerminate: (evt, gestureState) => {
        // Another component has become the responder, so this gesture
        // should be cancelled
      },
      onShouldBlockNativeResponder: (evt, gestureState) => {
        // Returns whether this component should block native components from becoming the JS
        // responder. Returns true by default. Is currently only supported on android.
        return true;
      },
    }),
  ).current;
 

  return (
    <>
          <View {...panResponder.panHandlers} style={{
            flex:1,
            backgroundColor:colors.green,
        }}></View>
        <View style={{
            flex:4,
            backgroundColor:colors.red,
            alignItems:'center',
            justifyContent:'center',
        }}>
        {isVisible?
        <TouchableOpacity onPress={init} style={{
          backgroundColor:colors.white,
          padding:4,
        }}>
          <Text>send</Text>
        </TouchableOpacity>
        :<></>}
        </View>
        <View {...otherPanResponder.panHandlers} style={{
            flex:1,
            backgroundColor:colors.green,
        }}></View>
    </>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  titleText: {
    fontSize: 14,
    lineHeight: 24,
    fontWeight: 'bold',
  },
  box: {
    height: 150,
    width: 150,
    backgroundColor: 'blue',
    borderRadius: 5,
  },
});

const colors = {
    green:'#c6f2bd',
    red:'#f8bd9e',
    yellow:'#fbf071',
    white:'#ffffff'
};

export default InputSender;