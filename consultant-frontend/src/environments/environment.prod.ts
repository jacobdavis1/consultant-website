export const environment = {
  production: true,

  auth0_domain: "dbnd.auth0.com",
  auth0_clientId: "kiu6Q4qgv94zYCW0xlVKlAp54J4by8sz",
  auth0_consultant_api_audience: "/consultant-api",
  auth0_managementAudience: "https://dbnd.auth0.com/api/v2/",
  auth0_managementToken: "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ik9FWkNOalF3TlVOR1JrUXdPVEpCTVVaQ09VVTVSVVExUlRJeFJFVkZPVEUzUkVFek5URXdSQSJ9.eyJpc3MiOiJodHRwczovL2RibmQuYXV0aDAuY29tLyIsInN1YiI6IkI4NllxTkNOd1R5U2YwSGY5U0plcm81VDhhaWVIcWxFQGNsaWVudHMiLCJhdWQiOiJodHRwczovL2RibmQuYXV0aDAuY29tL2FwaS92Mi8iLCJpYXQiOjE1OTM1NjQ0NjgsImV4cCI6MTU5MzY1MDg2OCwiYXpwIjoiQjg2WXFOQ053VHlTZjBIZjlTSmVybzVUOGFpZUhxbEUiLCJzY29wZSI6InJlYWQ6Y2xpZW50X2dyYW50cyBjcmVhdGU6Y2xpZW50X2dyYW50cyBkZWxldGU6Y2xpZW50X2dyYW50cyB1cGRhdGU6Y2xpZW50X2dyYW50cyByZWFkOnVzZXJzIHVwZGF0ZTp1c2VycyBkZWxldGU6dXNlcnMgY3JlYXRlOnVzZXJzIHJlYWQ6dXNlcnNfYXBwX21ldGFkYXRhIHVwZGF0ZTp1c2Vyc19hcHBfbWV0YWRhdGEgZGVsZXRlOnVzZXJzX2FwcF9tZXRhZGF0YSBjcmVhdGU6dXNlcnNfYXBwX21ldGFkYXRhIGNyZWF0ZTp1c2VyX3RpY2tldHMgcmVhZDpjbGllbnRzIHVwZGF0ZTpjbGllbnRzIGRlbGV0ZTpjbGllbnRzIGNyZWF0ZTpjbGllbnRzIHJlYWQ6Y2xpZW50X2tleXMgdXBkYXRlOmNsaWVudF9rZXlzIGRlbGV0ZTpjbGllbnRfa2V5cyBjcmVhdGU6Y2xpZW50X2tleXMgcmVhZDpjb25uZWN0aW9ucyB1cGRhdGU6Y29ubmVjdGlvbnMgZGVsZXRlOmNvbm5lY3Rpb25zIGNyZWF0ZTpjb25uZWN0aW9ucyByZWFkOnJlc291cmNlX3NlcnZlcnMgdXBkYXRlOnJlc291cmNlX3NlcnZlcnMgZGVsZXRlOnJlc291cmNlX3NlcnZlcnMgY3JlYXRlOnJlc291cmNlX3NlcnZlcnMgcmVhZDpkZXZpY2VfY3JlZGVudGlhbHMgdXBkYXRlOmRldmljZV9jcmVkZW50aWFscyBkZWxldGU6ZGV2aWNlX2NyZWRlbnRpYWxzIGNyZWF0ZTpkZXZpY2VfY3JlZGVudGlhbHMgcmVhZDpydWxlcyB1cGRhdGU6cnVsZXMgZGVsZXRlOnJ1bGVzIGNyZWF0ZTpydWxlcyByZWFkOnJ1bGVzX2NvbmZpZ3MgdXBkYXRlOnJ1bGVzX2NvbmZpZ3MgZGVsZXRlOnJ1bGVzX2NvbmZpZ3MgcmVhZDplbWFpbF9wcm92aWRlciB1cGRhdGU6ZW1haWxfcHJvdmlkZXIgZGVsZXRlOmVtYWlsX3Byb3ZpZGVyIGNyZWF0ZTplbWFpbF9wcm92aWRlciBibGFja2xpc3Q6dG9rZW5zIHJlYWQ6c3RhdHMgcmVhZDp0ZW5hbnRfc2V0dGluZ3MgdXBkYXRlOnRlbmFudF9zZXR0aW5ncyByZWFkOmxvZ3MgcmVhZDpzaGllbGRzIGNyZWF0ZTpzaGllbGRzIGRlbGV0ZTpzaGllbGRzIHJlYWQ6YW5vbWFseV9ibG9ja3MgZGVsZXRlOmFub21hbHlfYmxvY2tzIHVwZGF0ZTp0cmlnZ2VycyByZWFkOnRyaWdnZXJzIHJlYWQ6Z3JhbnRzIGRlbGV0ZTpncmFudHMgcmVhZDpndWFyZGlhbl9mYWN0b3JzIHVwZGF0ZTpndWFyZGlhbl9mYWN0b3JzIHJlYWQ6Z3VhcmRpYW5fZW5yb2xsbWVudHMgZGVsZXRlOmd1YXJkaWFuX2Vucm9sbG1lbnRzIGNyZWF0ZTpndWFyZGlhbl9lbnJvbGxtZW50X3RpY2tldHMgcmVhZDp1c2VyX2lkcF90b2tlbnMgY3JlYXRlOnBhc3N3b3Jkc19jaGVja2luZ19qb2IgZGVsZXRlOnBhc3N3b3Jkc19jaGVja2luZ19qb2IgcmVhZDpjdXN0b21fZG9tYWlucyBkZWxldGU6Y3VzdG9tX2RvbWFpbnMgY3JlYXRlOmN1c3RvbV9kb21haW5zIHJlYWQ6ZW1haWxfdGVtcGxhdGVzIGNyZWF0ZTplbWFpbF90ZW1wbGF0ZXMgdXBkYXRlOmVtYWlsX3RlbXBsYXRlcyByZWFkOm1mYV9wb2xpY2llcyB1cGRhdGU6bWZhX3BvbGljaWVzIHJlYWQ6cm9sZXMgY3JlYXRlOnJvbGVzIGRlbGV0ZTpyb2xlcyB1cGRhdGU6cm9sZXMgcmVhZDpwcm9tcHRzIHVwZGF0ZTpwcm9tcHRzIHJlYWQ6YnJhbmRpbmcgdXBkYXRlOmJyYW5kaW5nIiwiZ3R5IjoiY2xpZW50LWNyZWRlbnRpYWxzIn0.MiCJrCjE0-GK2DjCoOF_3aoTND9vRunO6pcIERmIR-Ykv7y3r0fXEQdTloT9rc6MzhI0oRVfc9XXZsxFRq-wra5hrv72BS29Hi9z11iA5C7FTdi4PvBnxlePHGg0HbGQ8OOyR7_S_AV8M4HBeUV90GSs3sE_7G1f4pgMLDDsOdzBQZV7Hc_awo3hs0kQ7aBY3NDrvaKG06BLvAHMquW0ULNbizv7Yzok06nNou2vT-wbWCcoqVOal5AfTeuPYJl_IvSIhxM9iF-3p-1uFx0v_S2_nfZRQBAdkCEOvCaUOx2LTKoET-z9wpKwajtWvgxAwVH7m-bR7URXyy5cSg9GLA",

  azure_accountName: "jacdfiles",
  azure_accountSas: "?sv=2019-02-02&ss=bfqt&srt=sco&sp=rwdlacup&se=2020-04-23T22:41:28Z&st=2020-04-23T14:41:28Z&spr=https&sig=BAdPQOCLWxqn9DFbU2pyEChmps3ThYWwLsPM07Eu%2BV4%3D",
  azure_shareName: "testhare"
};
