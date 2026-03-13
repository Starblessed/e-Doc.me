import 'dart:convert';

import 'package:http/http.dart' as http;

const String scannerUrl = "http://localhost:8000";

class ScanTaskResponse {
  final String taskId;
  final String status;

  ScanTaskResponse({required this.taskId, required this.status});

  factory ScanTaskResponse.fromMap(Map<String, dynamic> map) {
    return ScanTaskResponse(
      taskId: map['task_id'] as String,
      status: map['status'] as String,
    );
  }
}

class ScanTaskResult extends ScanTaskResponse {
  final String resultPath;

  ScanTaskResult({
    required super.taskId,
    required super.status,
    required this.resultPath,
  });

  factory ScanTaskResult.fromMap(Map<String, dynamic> map) {
    return ScanTaskResult(
      taskId: map['task_id'] as String,
      status: map['status'] as String,
      resultPath: map['result'] as String,
    );
  }
}

/// Submits the document at [path] for scanning and returns a task ID to fetch results.
///
/// The scanned document format is defined in [exportAs], set to pdf as the default.
///
/// Optionally, an alternative service URL can be provided as [serviceUrl].
Future<ScanTaskResponse> requestDocumentScan(
  String path, {
  String serviceUrl = scannerUrl,
  String exportAs = "png",
}) async {
  final url = Uri.parse("$serviceUrl/digitalize");
  final body = jsonEncode(<String, dynamic>{
    "img_path": path,
    "export_as": exportAs,
  });

  final response = await http.post(
    url,
    headers: {"Content-Type": "application/json"},
    body: body,
  );

  if (response.statusCode == 200) {
    //
    return ScanTaskResponse.fromMap(jsonDecode(response.body));
  } else {
    //
    throw Exception(
      "Failed to create post. Status code: ${response.statusCode}",
    );
  }
}

Future<ScanTaskResult> fetchDocumentScanResult(
  String taskId, {
  String serviceUrl = scannerUrl,
}) async {
  final url = Uri.parse("$serviceUrl/tasks/$taskId");

  final response = await http.get(
    url,
    headers: {"Content-Type": "application/json"},
  );

  if (response.statusCode == 200) {
    //
    print(jsonDecode(response.body));
    return ScanTaskResult.fromMap(jsonDecode(response.body));
  } else {
    //
    throw Exception(
      "Failed to fetch results. Status code: ${response.statusCode}",
    );
  }
}
