/// <reference path="types-gt-mp/index.d.ts" />

class GtaForecast {
	public entries: GtaForecastEntry[];
}

class GtaForecastEntry {
	public state: GtaWeatherStates;
	public dayTemp: number;
	public nightTemp: number;
	public dateTime?: Date;
}

enum GtaWeatherStates {
	Unspecified = -1,

	ExtraSunny = 0,
	Clear = 1,
	Clouds = 2,
	Smog = 3,
	Foggy = 4,
	Overcast = 5,
	Rain = 6,
	Thunder = 7,
	LightRain = 8,
	SmoggyLightRain = 9,
	VeryLightSnow = 10,
	WindyLightSnow = 11,
	LightSnow = 12,
	Unknown = 13
}

const Weekdays: string[] = [
	"Sunday",
	"Monday",
	"Tuesday",
	"Wednesday",
	"Thursday",
	"Friday",
	"Saturday"
]

API.onServerEventTrigger.connect((eventName: string, _arguments: any[]): void => {
	switch (eventName) {
		case 'OWM_FORECAST':
			const forecast: GtaForecast = JSON.parse(_arguments[0]);
			const todayDate: number = (new Date()).getDate();

			let todayIndex = 0;
			for (let i = 0; i < forecast.entries.length; i++) {
				if (forecast.entries[i].dateTime.getDate() == todayDate) {
					todayIndex = i;
					break;
				}
			}

			sendWeatherNotification("today", forecast.entries[todayIndex]);
			sendWeatherNotification("tomorrow", forecast.entries[todayIndex + 1]);
			sendWeatherNotification(Weekdays[forecast.entries[todayIndex + 2].dateTime.getDay()], forecast.entries[todayIndex + 2]);
			sendWeatherNotification(Weekdays[forecast.entries[todayIndex + 3].dateTime.getDay()], forecast.entries[todayIndex + 3]);
			
			break;
	}
});

function sendWeatherNotification(when: string, forecastEntry: GtaForecastEntry): void {
	API.sendNotification("Weather " + when + ": " + forecastEntry.state + " with temperatures between " + forecastEntry.dayTemp.toFixed(0) + "°C and " + forecastEntry.nightTemp.toFixed(0) + "°C");
}