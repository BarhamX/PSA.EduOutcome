import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'EduOutcome',
  },
  apis: {
    default: {
      url: 'https://localhost:44354',
      rootNamespace: 'PSA.EduOutcome',
    },
  },
  oAuthConfig: {
    issuer: 'https://localhost:44354',
    tokenEndpoint: 'https://localhost:44354/connect/token',
    clientId: 'EduOutcome_App',
    scope: 'offline_access openid profile email roles EduOutcome',
    redirectUri: baseUrl,
    postLogoutRedirectUri: baseUrl,
  },
} as Environment;

