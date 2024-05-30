import React, { useState } from "react";
import { View, TouchableOpacity,Text} from "react-native";
import dgram from 'react-native-udp';
import { getIpAddress } from "react-native-device-info";

const Home = ({navigation,route})=>{

    const [message,setMessage]=useState("Press the button to start scanning for devices");
    const handlePress= ()=>{
        // getIpAddress().then((ip) => {
        //     // "92.168.32.44"
        //     setMessage(ip);
        //   });
        const socket = dgram.createSocket('udp4');
        socket.bind(11001);

        socket.on('message',(msg,rinfo)=>{
            //socket.send()
            const msgg = JSON.stringify(msg);
            console.log(msgg);
            const stringDataArray = (JSON.parse(msgg).data);
            let stringData="";
            stringDataArray.forEach((data,index)=>{
                stringData+=String.fromCharCode(data);
            });

            if(stringData=="VJOY_FEEDER_AVAILABLE"){
                socket.close();
                navigation.navigate('inputSender',{ip:rinfo.address});
            }
         

        });
        //navigation.navigate('inputSender',{data:'fuck wucky woo woo'});
        //this function navigates you to the next page
    }

    return(<>

        <View  style={{
            flex:1,
            backgroundColor:colors.green,
        }}></View>
        <View style={{
            flex:4,
            backgroundColor:colors.red,
            alignItems:'center',
            justifyContent:'center',
        }}><View style={{alignItems:'center',backgroundColor:colors.green}}>
            <TouchableOpacity style={{
                backgroundColor:'#ffffff',
                padding:5,
                borderRadius:5,
                borderWidth:1,
            }}
            onPress={handlePress}
            >
                <Text>Scan</Text>
            </TouchableOpacity>
            <View style={{backgroundColor:colors.yellow,marginTop:10}}><Text>{message}</Text></View>
            </View>
        </View>
        <View style={{
            flex:1,
            backgroundColor:colors.green,
        }}></View>
    </>);
}


const colors = {
    green:'#c6f2bd',
    red:'#f8bd9e',
    yellow:'#fbf071',
};


const remoteHost="192.168.137.255";
const remotePort=11000;

export default Home;