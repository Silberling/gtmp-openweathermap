# gtmp-openweathermap

## Description

Gamemode for setting ingame weather to real weather of a city of your choice in defined time intervals.

## Installation

1. Place the OpenWeatherMap folder in the resources directory
2. Edit settings.xml and add
```<resource src="OpenWeatherMap" />```
3. Sign up at http://openweathermap.org/ and get an appid
4. Edit meta.xml and set the value of setting name="owm_appId"

## Using Forecasts

Send OWM_GET_FORECAST with no content. You will receive OWM_FORECAST with a serialized JSON object. See the example included.

## Contribution

If you find bugs, just tell me. If you'd like to improve or extend the code, just fork this repo and share it back.
