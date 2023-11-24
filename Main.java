import java.util.*;

public class Main {
    public static void main(String[] args) {
        HashMap<String, List<String>> phoneBook = new HashMap<>();

        addContact(phoneBook, "Ivan", "123-456-7890");
        addContact(phoneBook, "Mariy", "987-654-3210");
        addContact(phoneBook, "Ivan", "111-222-3333");
        addContact(phoneBook, "Anna", "555-555-5555");
        addContact(phoneBook, "Andrey", "777-777-7777");

        // Выводим телефонную книгу
        printPhoneBook(phoneBook);
    }

    // Метод для добавления контакта в телефонную книгу
    public static void addContact(HashMap<String, List<String>> phoneBook, String name, String phoneNumber) {
        if (phoneBook.containsKey(name)) {
            phoneBook.get(name).add(phoneNumber);
        } else {
            List<String> phoneNumbers = new ArrayList<>();
            phoneNumbers.add(phoneNumber);
            phoneBook.put(name, phoneNumbers);
        }
    }

    // Метод для вывода телефонной книги в порядке убывания числа телефонов
    public static void printPhoneBook(HashMap<String, List<String>> phoneBook) {
        List<HashMap.Entry<String, List<String>>> sortedEntries = new ArrayList<>(phoneBook.entrySet());
        sortedEntries.sort((entry1, entry2) -> Integer.compare(entry2.getValue().size(), entry1.getValue().size()));

        for (HashMap.Entry<String, List<String>> entry : sortedEntries) {
            System.out.println(entry.getKey() + ": " + entry.getValue());
        }
    }
}
