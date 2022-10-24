import axios from 'axios';

const instance = axios.create({
    baseURL: process.env.BACKEND_URL,
    timeout: 25000
});

export default instance;