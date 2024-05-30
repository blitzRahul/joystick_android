import React from 'react';
import {View,Text, TouchableOpacity} from 'react-native';
import { Colors } from 'react-native/Libraries/NewAppScreen';
import { useState } from 'react';
import {NavigationContainer} from '@react-navigation/native';
import {createNativeStackNavigator} from '@react-navigation/native-stack';
import InputSender from './InputSender';
import Home from './Home';

const Stack = createNativeStackNavigator();


const App=  ()=>{

  return (
<>
  <NavigationContainer>
  <Stack.Navigator screenOptions={{headerShown:false}}>

  <Stack.Screen
      name='home'
      component={Home}
    
    />
    <Stack.Screen
      name='inputSender'
      component={InputSender}
    />

   

  </Stack.Navigator>
  </NavigationContainer>
</>
  );
};




const TestComp= ({navigation,route})=>{
//navigation.navigate('name',{data:'dt'})
//route.params.param_name (params passed post routing)

return(
  <>
  <View style={{
    flex:1,
    alignItems:'center',
    justifyContent:'center',
  }}>
  <Text>Hello world testing navigation</Text>
  </View>
  </>
);
};

export default App;