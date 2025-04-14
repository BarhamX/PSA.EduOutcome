import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44354/',
  redirectUri: baseUrl,
  clientId: 'EduOutcome_App',
  responseType: 'code',
  scope: 'offline_access EduOutcome',
  requireHttps: true,
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'EduOutcome',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44354',
      rootNamespace: 'PSA.EduOutcome',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge'
  }
} as Environment;
