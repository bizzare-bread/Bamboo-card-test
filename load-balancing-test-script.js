import http from 'k6/http';
import { sleep } from 'k6';

// test options
export let options = {
    vus: 250, // Number of virtual users (concurrent requests)
    duration: '10s', // Duration of the test
};

// test logic
export default function () {
    const response = http.get('http://localhost:5088/weatherforecast');
}