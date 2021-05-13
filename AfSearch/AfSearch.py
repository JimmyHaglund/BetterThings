import json
import requests
from datetime import date
from pathlib import Path
import os
import copy

# https://jobsearch.api.jobtechdev.se/

url = "https://jobsearch.api.jobtechdev.se/"

headers = {'api-key': '', 'accept': 'application/json'}
url_for_search = f"{url}/search"
base_search_params = {
    'limit': 10, 
    'published-after': 1440, # 1440 minutes = 1 day, 10040 = 1 week
    'experience': 'false'
}

def get_api_key():
    with open('AfApiKey.txt') as infile:
        return infile.readline()

def get_headers():
    if (headers['api-key'] == ''):
        headers['api-key'] = get_api_key()
    return headers

def search_with_params(search_params):
    response = requests.get(url_for_search, headers=get_headers(), params=search_params)
    response.raise_for_status()  # check for http errors
    json_response = json.loads(response.content.decode('utf8'))
    return json_response['hits']

def search_query(query, limit):
    search_params = {'q': query, 'limit': limit }
    response = requests.get(url_for_search, headers=headers, params=search_params)
    response.raise_for_status()  # check for http errors
    json_response = json.loads(response.content.decode('utf8'))
    return json_response['hits']

def output_to_json(file_path, data):
    with open(file_path, 'w') as outfile:
        json.dump(data, outfile, indent=2)

def search_industry_and_construction():
    search_params = get_base_search_params()
    search_params['occupation-field'] = [2, 9] # 2 = Bygg & konstruktion, 9 = Industriell Tillverkning
    search_params['region'] = 14 # 14 = Västra Götaland
    return search_with_params(search_params)

def search_janitor():
    search_params = get_base_search_params()
    search_params['occupation-name'] = 468 # 468 = Städ & Lokalvård
    search_params['region'] = 14 # 14 = Västra Götaland
    return search_with_params(search_params)

def search_vaktmaster():
    search_params = get_base_search_params()
    search_params['occupation-group'] = 9622 # 9622 = Vaktmästare m.fl.
    search_params['region'] = 14 # 14 = Västra Götaland
    return search_with_params(search_params)

def search_programmer():
    search_params = get_base_search_params()
    search_params['occupation-field'] = 3
    search_params['limit'] = 50
    return search_with_params(search_params)

# q = freetext query
# published-after = number of minutes back in time to perform search.
# published-after Can also be a date in the form YYYY-mm-ddTHH:MM:SS
# limit = max number of results, 0 will only return the number of results an not the results themselves
# region = region code. 14 is Västra Götaland
# occupation-field = proffesion field code. 3 is data/it
def get_base_search_params():
    return copy.copy(base_search_params)

def extract_relevant_data(result_data):
    return {
        'Headline': result_data['headline'],
        'Occupation': result_data['occupation']['label'] + ' | ' + result_data['occupation']['legacy_ams_taxonomy_id'],
        'OccupationGroup': result_data['occupation_group']['label'] + ' | ' + result_data['occupation_group']['legacy_ams_taxonomy_id'],
        'OccupationField': result_data['occupation_field']['label'] + ' | ' + result_data['occupation_field']['legacy_ams_taxonomy_id'],
        'Municipality': result_data['workplace_address']['municipality'],
        'Town': result_data['workplace_address']['city'],
        'ApplicationDeadline': result_data['application_deadline'],
        'AfUrl': result_data['webpage_url'],
        'Recruiter': result_data['employer']['name'],
        'Workplace': result_data['employer']['workplace'],
        'ApplicationEmail': result_data['application_details']['email'],
        'ApplicationUrl': result_data['application_details']['url'],
        'Description': result_data['description']['text']
    }

def compile_results(all_results):
    distilled_results = []
    for results in all_results:
        for result in results:
            distilled_results.append(extract_relevant_data(result))
    return distilled_results

def ensure_folder_exists(folder_path):
    if not os.path.exists(folder_path):
        try:
            os.makedirs(folder_path)
        except Exception as e:
            print(e)
            raise

def get_todays_file_path(file_name):
    return os.path.join(get_todays_folder_name(), file_name + '.json')

def get_todays_tattarjobb_raw_path():
    return get_todays_file_path('raw_tattarjobb')

def get_todays_tattarjobb_output_path():
    return get_todays_file_path('output_tattarjobb')

def get_todays_programmer_output_path():
    return get_todays_file_path('output_programmerare')

def get_todays_programmer_raw_path():
    return get_todays_file_path('raw_programmerare')

def get_todays_folder_name():
    return os.path.join('Results', str(date.today()))

def compile_tattarjobb():
    result_industry_and_construction = search_industry_and_construction()
    result_janitor = search_janitor()
    result_vaktmaster = search_vaktmaster()
    raw_name = get_todays_tattarjobb_raw_path()
    output_name = get_todays_tattarjobb_output_path()

    distilled_result = compile_results([result_industry_and_construction, result_janitor, result_vaktmaster])
    
    ensure_folder_exists(get_todays_folder_name())
    output_to_json(raw_name, [result_industry_and_construction, result_janitor, result_vaktmaster])
    output_to_json(output_name, distilled_result)

def compile_programmer():
    result = search_programmer()
    raw_name = get_todays_programmer_raw_path()
    output_name = get_todays_programmer_output_path()
    
    distilled_result = compile_results([result])

    ensure_folder_exists(get_todays_folder_name())
    output_to_json(raw_name, result)
    output_to_json(output_name, distilled_result)


if __name__ == '__main__':
    compile_tattarjobb()
    compile_programmer()