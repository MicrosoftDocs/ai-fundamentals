def get_form_data(key, endpoint, form_data):
    import json
    import time
    from requests import get, post

    results = {}

    # get endpoint for form recognizer service
    post_url = endpoint + "/formrecognizer/v2.0-preview/prebuilt/receipt/analyze"

    # Create HTTP request headers and params
    headers = {
        'Content-Type': 'image/jpeg',
        'Ocp-Apim-Subscription-Key': key,
    }
    params = {
        "includeTextDetails": True
    }

    # Submit the request to Form Recognizer
    resp = post(url = post_url, data = form_data, headers = headers, params = params)
    if resp.status_code != 202:
        raise ValueError('Failed to submit request')
    print("Request submitted. Awaiting analysis results...")
    # get the URL for the asynchronous results
    get_url = resp.headers["operation-location"]

    # Now we need to poll for the results
    n_tries = 5
    n_try = 0
    wait_sec = 2
    complete = False
    while n_try < n_tries and complete != True:
        # get the response
        resp = get(url = get_url, headers = {"Ocp-Apim-Subscription-Key": key})
        resp_json = json.loads(resp.text)
        if resp.status_code != 200:
            raise ValueError('Failed to process results')
            complete = True
        status = resp_json["status"]
        if status == "succeeded":
            print('Results retrieved...')
            # Parse the fields in the response JSON
            fields = resp_json["analyzeResult"]["documentResults"][0]["fields"]
            for field in fields:
                # What type of field is this?
                fieldType = fields[field]["type"]
                if fieldType != 'array':
                    # This has a single value, so work out the name and data type
                    fieldTypeString = fieldType[0].capitalize() + fieldType[1:]
                    valueField = 'value' + fieldTypeString
                    results[field] = fields[field][valueField]
                else:
                    # This is an array - is it Items?
                    if field == "Items":
                        # Items are line-items in the receipt, get the name and price for each
                        items = fields[field]
                        for item in items["valueArray"]:
                            results[item["valueObject"]["Name"]["valueString"]] = item["valueObject"]["TotalPrice"]["valueNumber"]
            complete = True
        if status == "failed":
            raise ValueError('Failed to process results')
            complete = True
        # Analysis still running. Wait and retry.
        time.sleep(wait_sec)
        n_try += 1
    # Return the results we found
    return results
