import React, { useState } from "react";
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

export const url = "http://localhost:5063/";

export enum ENDPOINT {
  LOGIN = "ApiLogin",
  REGISTER = "ApiRegister",
  PATIENTS = "ApiPatients",
  DOCTORS = "ApiDoctors",
  SCHEDULE = "ApiSchedule",
  SCHEDULE_DATES = "ApiSchedule/dates",
  SCHEDULE_REMOVE = "ApiSchedule/remove",
  SCHEDULE_ADD = "ApiSchedule/add",
  SCHEDULE_SELECT = "ApiSchedule/select",
  SCHEDULE_COPY_WEEK = "ApiSchedule/copy_week",
  SCHEDULE_SPECIALIZATION = "ApiSchedule/specialization",
  SCHEDULE_PLAN = "ApiSchedule/plan",
  SCHEDULE_GET_PLANNED = "ApiSchedule/get_planned",
  SCHEDULE_CANCEL = "ApiSchedule/cancel",
  SCHEDULE_GET_DOCTOR_SCHEDULED = "ApiSchedule/get_doctor_scheduled",
  SCHEDULE_GET_DOCTOR_VISITS = "ApiSchedule/get_doctor_visits",
  SCHEDULE_GET_VISIT_HISTORY = "ApiSchedule/get_history",
  SCHEDULE_ADD_DESC = "ApiSchedule/add_desc",
  REPORT = "ApiReport",
}

export type METHOD = "GET" | "POST" | "PATCH" | "DELETE";

export default function useAPIFetch<DataType>(
  endpoint: ENDPOINT,
  method: METHOD = "GET"
): [
    (data: any, ...queryParams: Array<string | number>) => void,
    number,
    DataType,
    boolean,
    React.Dispatch<React.SetStateAction<number>>
  ] {
  const [status, setStatus] = useState<number>(0);
  const [data, setData] = useState<DataType>();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const navigate = useNavigate();

  const execute = async (data: any, ...queryParams: Array<string | number>) => {
    console.log(data);
    setIsLoading(true);

    try {
      const response = await axios({
        method: method,
        url: `${url}${endpoint}/${queryParams.join("/")}`,
        headers: {
          "Access-Control-Allow-Origin": "*",
          "Access-Control-Allow-Methods": "DELETE, POST, GET, OPTIONS",
          "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
          "Content-Type": "application/json",
        },
        data: data !== undefined ? JSON.stringify(data) : data,
        withCredentials: true,
      });

      console.log("Response:", response);
      setStatus(response.status);
      setData(response.data);
      setIsLoading(false);
    } catch (error) {
      if (error.response.status === 403) {
        navigate('/AccessDenied');
      } else if (error.response.status === 400 || error.response.status === 405 || error.response.status === 500) {
        navigate('/Error');
      }
      console.error("Error:", error);
      setStatus(400);
      setIsLoading(false);
    }
  };

  return [execute, status, data as DataType, isLoading, setStatus];
}
