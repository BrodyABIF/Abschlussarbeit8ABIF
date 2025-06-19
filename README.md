# Abschlussarbeit8ABIF
Finale Abgabe für das letzte Semester.


Implementieren Sie eine REST API in einem ASP.NET Core Web API Projekt. Diese Applikation muss eine eigenständige Implementierung und somit eine Implementierung unabhängig von den Beispielen bzw. Übungsaufgaben des Unterrichts umfassen. Sie können als Ausgangsbasis die Implementierung aus dem Wintersemester verwenden oder stattdessen einfache Modelklassen implementieren.

Mindestanforderungen:

    Eine Controller Klasse beinhaltet die Methoden für folgende Routen.
    Eine GET Route ohne Parameter liefert die in einer Datenbank vorhandenen Instanzen einer Modelklasse, wobei eine Liste einer zu implementierenden DTO Klasse zurückgeliefert wird.
    Eine GET Route mit einem Schlüssel als Parameter liefert die Detaildaten zu dem Objekt mit dem übergebenen Schlüssel zurück, wobei für die Detaildaten eine DTO Klasse, welche zumindest eine weitere DTO Klasse eingebettet hat, implementiert ist.
    Eine POST Route für das Anlegen einer Objektinstanz verwendet eine Command Klasse für die einzufügenden Daten.
    Eine PATCH Route für das Ändern einer Objektinstanz verwendet eine Command Klasse für die neuen Daten und liefert Informationen zum veränderten Objekt zurück, wobei dafür eine DTO Klasse verwendet wird.
    Eine Delete Route für das Löschen einer Objektinstanz ist implementiert.
    Alle Routen liefern den Statuscode 200 (OK) im Erfolgsfall und den Statuscode 404 (Not Found), wenn ein Objekt mit dem übergebenen Schlüssel nicht gefunden wurde, zurück.
    Für alle Routen und für alle möglichen Statuscodes, die zurückgeliefert werden können, steht jeweils zumindest ein Integration Test zur Verfügung und läuft fehlerfrei durch.

Erweiterungsmöglichkeiten für eine bessere Beurteilung:

    Ein übergebener Parameter wird mittels einer Regular Expression überprüft und im Fehlerfall wird der Statuscode 400 (Bad Request) zurückgeliefert.
    Eine GET Route hat einen optionalen Paramter vom Datentyp Boolean und liefert abhängig von diesem Parameter unterschiedliche Ergebnisse zurück.
    Eine Delete Route für das Löschen einer Objektinstanz liefert den Statuscode 400 (Bad Request) zurück, wenn das Objekt noch Referenzen zu verbundenen Objekten aufweist.
    Eine DTO Klasse beinhaltet eine weitere DTO Klasse und diese beinhaltet wiederum eine weitere DTO Klasse (mindestens 2 Ebenen).
    In Command Klassen sind Validierungen implementiert.
    Die Routen liefern RFC-9457 ProblemDetails im Body der Antwort zurück.
    Weitere Integration Tests sind implementiert.
